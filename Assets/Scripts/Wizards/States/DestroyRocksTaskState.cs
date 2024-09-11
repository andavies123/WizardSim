using System;
using System.Collections.Generic;
using Extensions;
using GameWorld.WorldObjects.Rocks;
using StateMachines;
using UnityEngine;

namespace Wizards.States
{
	public class DestroyRocksTaskState : WizardTaskState
	{
		private readonly List<Rock> _rocks;
		private readonly StateMachine _stateMachine = new();
		private readonly int _initialRockCount;
		
		private readonly WizardMoveToState _moveToState;
		private readonly DestroyRockState _destroyRockState;

		public override event EventHandler<string> ExitRequested;
		
		public DestroyRocksTaskState(List<Rock> rocks)
		{
			_moveToState = new WizardMoveToState(Wizard);
			_destroyRockState = new DestroyRockState(Wizard);
			
			_rocks = rocks;
			_initialRockCount = rocks.Count;
			UpdateDisplayStatus();
			AddStateTransitions();
		}

		public override string DisplayName => $"Destroying {_rocks.Count} Rocks";
		public override string DisplayStatus { get; protected set; }

		public override void Begin()
		{
			_moveToState.SetWizard(Wizard);
			_moveToState.Initialize(_rocks[0].transform.position, 2f);
			_stateMachine.SetCurrentState(_moveToState);
		}

		public override void Update()
		{
			_stateMachine.Update();
			DisplayStatus = $"Rocks Destroyed: {_initialRockCount - _rocks.Count} of {_initialRockCount} | {_stateMachine.CurrentStateDisplayStatus}";
		}

		public override void End() { }

		private void AddStateTransitions()
		{
			_stateMachine.AddStateTransition(
				new StateTransitionKey(_moveToState, WizardMoveToState.EXIT_REASON_ARRIVED_AT_POSITION),
				new StateTransitionValue(_destroyRockState, InitializeDestroyRockState, () => true));
			
			_stateMachine.AddStateTransition(
				new StateTransitionKey(_destroyRockState, DestroyRockState.EXIT_REASON_ROCK_DESTROYED),
				new StateTransitionValue(_moveToState, InitializeMoveToState, () => true));
		}
		
		private void UpdateDisplayStatus()
		{
			DisplayStatus = $"Rocks Destroyed: {_initialRockCount - _rocks.Count}/{_initialRockCount}";
		}
		
		private void InitializeDestroyRockState()
		{
			_destroyRockState.Initialize(_rocks[0]);
		}

		private void InitializeMoveToState()
		{
			_rocks.RemoveAt(0);
			UpdateDisplayStatus();
			
			if (_rocks.IsEmpty())
			{
				CompleteTask();
				return;
			}

			Rock rock = _rocks[0];
			_moveToState.Initialize(rock.transform.position, 2f);
		}
	}
}