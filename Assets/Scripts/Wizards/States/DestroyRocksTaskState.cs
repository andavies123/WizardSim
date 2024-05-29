using System;
using System.Collections.Generic;
using GameWorld.WorldObjects.Rocks;
using StateMachines;
using Utilities;

namespace Wizards.States
{
	public class DestroyRocksTaskState : WizardTaskState
	{
		private readonly List<Rock> _rocks;
		private readonly StateMachine<WizardState> _stateMachine = new();
		private readonly int _initialRockCount;
		
		private WizardMoveToState _moveToState;
		private DestroyRockState _destroyRockState;

		public DestroyRocksTaskState(List<Rock> rocks)
		{
			_rocks = rocks;
			_initialRockCount = rocks.Count;
			UpdateDisplayStatus();
		}
		
		public override string DisplayName => $"Destroying {_rocks.Count} Rocks";
		public override string DisplayStatus { get; protected set; }
		
		public override void Begin()
		{
			_moveToState = new WizardMoveToState(Wizard);
			_destroyRockState = new DestroyRockState(Wizard);

			_moveToState.ArrivedAtPosition += OnArrivedAtRock;
			_destroyRockState.RockDestroyed += OnRockDestroyed;
            
			ChangeToMoveToState();
		}

		public override void Update()
		{
			_stateMachine.Update();
			DisplayStatus = $"Rocks Destroyed: {_initialRockCount - _rocks.Count} of {_initialRockCount} | " + _stateMachine.CurrentStateDisplayStatus;
		}

		public override void End() { }

		private void ChangeToMoveToState()
		{
			if (_rocks.IsEmpty())
			{
				Complete();
				return;
			}

			Rock rock = _rocks[0];
			_moveToState.Initialize(rock.transform.position, 2f);
			_stateMachine.SetCurrentState(_moveToState);
		}

		private void ChangeToDestroyRockState()
		{
			_destroyRockState.Initialize(_rocks[0]);
			_stateMachine.SetCurrentState(_destroyRockState);
		}

		private void UpdateDisplayStatus()
		{
			DisplayStatus = $"Rocks Destroyed: {_initialRockCount - _rocks.Count}/{_initialRockCount}";
		}
		
		private void OnArrivedAtRock(object sender, EventArgs args)
		{
			ChangeToDestroyRockState();
		}

		private void OnRockDestroyed(object sender, EventArgs args)
		{
			_rocks.RemoveAt(0);
			UpdateDisplayStatus();
			ChangeToMoveToState();
		}
	}
}