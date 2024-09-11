using GeneralBehaviours;
using UnityEngine;
using Wizards.States;

namespace Wizards
{
	[RequireComponent(typeof(Wizard))]
	public class WizardStateMachine : StateMachineComponent
	{
		[Header("Idle State Defaults")]
		[SerializeField] private float idleRadius = 10;
		
		private Wizard _wizard;
		private WizardIdleState _idleState;
		private WizardMoveToState _moveToState;

		public void OverrideCurrentState(WizardState state)
		{
			StateMachine.SetCurrentState(state);
		}
		
		public void Idle()
		{
			_idleState.IdleRadius = idleRadius;
			StateMachine.SetCurrentState(_idleState);
		}

		public void MoveTo(Vector3 moveToPosition)
		{
			_moveToState.Initialize(moveToPosition, 0.5f);
			StateMachine.SetCurrentState(_moveToState);
		}

		private void Awake()
		{
			_wizard = GetComponent<Wizard>();

			_idleState = new WizardIdleState(_wizard);
			_moveToState = new WizardMoveToState(_wizard);

			StateMachine.DefaultState = _idleState;
		}

		private void Update()
		{
			StateMachine.Update();
		}
	}
}