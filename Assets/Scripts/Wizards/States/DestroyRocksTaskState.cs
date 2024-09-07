using System;
using System.Collections.Generic;
using Extensions;
using GameWorld.WorldObjects.Rocks;
using StateMachines;

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
			
			_stateMachine.AddStateTransition(
				_moveToState, WizardMoveToState.EXIT_REASON_ARRIVED_AT_POSITION, 
				_destroyRockState, OnArrivedAtRock);
			
			_stateMachine.AddStateTransition(
				_destroyRockState, DestroyRockState.EXIT_REASON_ROCK_DESTROYED,
				_moveToState, OnRockDestroyed);
		}

		public override string DisplayName => $"Destroying {_rocks.Count} Rocks";
		public override string DisplayStatus { get; protected set; }
		
		public override void Begin() { }

		public override void Update()
		{
			_stateMachine.Update();
			DisplayStatus = $"Rocks Destroyed: {_initialRockCount - _rocks.Count} of {_initialRockCount} | {_stateMachine.CurrentStateDisplayStatus}";
		}

		public override void End() { }
		
		private void UpdateDisplayStatus()
		{
			DisplayStatus = $"Rocks Destroyed: {_initialRockCount - _rocks.Count}/{_initialRockCount}";
		}
		
		private void OnArrivedAtRock()
		{
			_destroyRockState.Initialize(_rocks[0]);
		}

		private void OnRockDestroyed()
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