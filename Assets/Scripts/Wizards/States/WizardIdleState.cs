using System;
using StateMachines;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wizards.States
{
	public class WizardIdleState : WizardState
	{
		private readonly StateMachine _stateMachine = new();
		
		private readonly WizardWaitState _waitState;
		private readonly WizardMoveToState _moveToState;

		private Vector3 _centerIdlePosition;

		public WizardIdleState(Wizard wizard) : base(wizard)
		{
			_waitState = new WizardWaitState(wizard);
			_moveToState = new WizardMoveToState(wizard);

			_waitState.WaitFinished += OnIdleWaitFinished;
			_moveToState.ArrivedAtPosition += OnMoveToArrivedAtPosition;
		}

		public override string DisplayName => "Idling";
		public override string DisplayStatus { get; protected set; }
		public float IdleRadius { get; set; }

		public override void Begin()
		{
			_stateMachine.SetCurrentState(_waitState);
		}

		public override void Update()
		{
			_stateMachine.Update();
			DisplayStatus = $"{_stateMachine.CurrentStateDisplayName} - {_stateMachine.CurrentStateDisplayStatus}";
		}
		
		public override void End() { }

		private void OnIdleWaitFinished() => ChangeToMoveState();
		private void OnMoveToArrivedAtPosition(object sender, EventArgs args) => ChangeToWaitState();

		private Vector3 GetNextMoveToPosition()
		{
			Vector2 randomUnitCircle = Random.insideUnitCircle * IdleRadius;
			return new Vector3((int)(randomUnitCircle.x + _centerIdlePosition.x), 1, (int)(randomUnitCircle.y + _centerIdlePosition.z));
		}
		
		private static float GetRandomWaitTime() => Random.Range(5, 10);

		private void ChangeToWaitState()
		{
			_waitState.WaitTime = GetRandomWaitTime();
			_stateMachine.SetCurrentState(_waitState);
		}

		private void ChangeToMoveState()
		{
			_moveToState.Initialize(GetNextMoveToPosition(), .5f);
			_stateMachine.SetCurrentState(_moveToState);
		}
	}
}