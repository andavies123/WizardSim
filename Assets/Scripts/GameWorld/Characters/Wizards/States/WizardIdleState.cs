using System;
using GameWorld.Characters;
using GameWorld.Characters.States;
using StateMachines;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameWorld.Characters.Wizards.States
{
	public class WizardIdleState : WizardState
	{
		private readonly StateMachine _stateMachine = new();
		private readonly WaitCharacterState _waitState;
		private readonly MoveToPositionCharacterState _moveToState;

		private Vector3 _centerIdlePosition;
		
		public override event EventHandler<string> ExitRequested;

		public WizardIdleState(Wizard wizard) : base(wizard)
		{
			_waitState = new WaitCharacterState(wizard);
			_moveToState = new MoveToPositionCharacterState(wizard);
			
			_stateMachine.DefaultState = _waitState;
			
			InitializeStateTransitions();
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

		private void InitializeStateTransitions()
		{
			_stateMachine.AddStateTransition(
				new StateTransitionFrom(_waitState, WaitCharacterState.EXIT_REASON_DONE_WAITING),
				new StateTransitionTo(_moveToState, OnWaitTimerFinished, () => true));
			
			_stateMachine.AddStateTransition(
				new StateTransitionFrom(_moveToState, MoveToPositionCharacterState.EXIT_REASON_ARRIVED_AT_POSITION),
				new StateTransitionTo(_waitState, OnArrivedAtPosition, () => true));
		}

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