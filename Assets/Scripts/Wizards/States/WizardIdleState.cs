using System;
using GameWorld;
using StateMachines;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wizards.States
{
	public class WizardIdleState : WizardState
	{
		private readonly StateMachine _stateMachine = new();
		
		private readonly WaitCharacterState _waitState;
		private readonly WizardMoveToState _moveToState;

		private Vector3 _centerIdlePosition;
		
		public override event EventHandler<string> ExitRequested;

		public WizardIdleState(Wizard wizard) : base(wizard)
		{
			_waitState = new WaitCharacterState(wizard);
			_moveToState = new WizardMoveToState(wizard);

			_stateMachine.AddStateTransition(
				_waitState, WaitCharacterState.EXIT_REASON_DONE_WAITING, 
				_moveToState, OnWaitTimerFinished);
			_stateMachine.AddStateTransition(
				_moveToState, WizardMoveToState.EXIT_REASON_ARRIVED_AT_POSITION, 
				_waitState, OnArrivedAtPosition);
		}

		public override string DisplayName => "Idling";
		public override string DisplayStatus { get; protected set; }
		public float IdleRadius { get; set; }

		public override void Begin()
		{
			_waitState.WaitTime = GetRandomWaitTime();
			_stateMachine.SetCurrentState(_waitState);
		}

		public override void Update()
		{
			_stateMachine.Update();
			DisplayStatus = $"{_stateMachine.CurrentStateDisplayName} - {_stateMachine.CurrentStateDisplayStatus}";
		}
		
		public override void End() { }

		private void OnWaitTimerFinished() => _moveToState.Initialize(GetNextMoveToPosition(), .5f);
		private void OnArrivedAtPosition() => _waitState.WaitTime = GetRandomWaitTime();

		private Vector3 GetNextMoveToPosition()
		{
			Vector2 randomUnitCircle = Random.insideUnitCircle * IdleRadius;
			return new Vector3((int)(randomUnitCircle.x + _centerIdlePosition.x), 1, (int)(randomUnitCircle.y + _centerIdlePosition.z));
		}
		
		private static float GetRandomWaitTime() => Random.Range(5, 10);
	}
}