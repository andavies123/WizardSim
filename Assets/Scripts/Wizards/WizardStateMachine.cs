using GeneralBehaviours;
using StateMachines;
using System;
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
		private RunAwayWizardState _runAwayState;

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
			_runAwayState = new RunAwayWizardState(_wizard, 10f);

			StateMachine.DefaultState = _idleState;
		}

		private void Start()
		{
			_wizard.OnDamageDealt += OnWizardHurt;

			StateMachine.AddStateTransition(
				new StateTransitionKey(_runAwayState, RunAwayWizardState.EXIT_REASON_GOT_AWAY),
				new StateTransitionValue(_idleState, null, () => true));
		}

		private void OnDestroy()
		{
			_wizard.OnDamageDealt -= OnWizardHurt;
		}

		private void Update()
		{
			StateMachine.Update();
		}

		private void OnWizardHurt(object sender, EventArgs args)
		{
			StateMachine.SetCurrentState(_runAwayState);
		}
	}
}