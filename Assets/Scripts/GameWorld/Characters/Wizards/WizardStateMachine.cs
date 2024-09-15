using GeneralBehaviours;
using GeneralBehaviours.Damageable;
using StateMachines;
using UnityEngine;
using GameWorld.Characters.Wizards.States;

namespace GameWorld.Characters.Wizards
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
		private AttackWizardState _attackState;

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
			_attackState = new AttackWizardState(_wizard);

			StateMachine.DefaultState = _idleState;
		}

		private void Start()
		{
			_wizard.Damageable.DamageReceived += OnWizardReceivedDamage;

			StateMachine.AddStateTransition(
				new StateTransitionKey(_runAwayState, RunAwayWizardState.EXIT_REASON_GOT_AWAY),
				new StateTransitionValue(_idleState, null, () => true));

			StateMachine.AddStateTransition(
				new StateTransitionKey(_attackState, AttackWizardState.EXIT_REASON_TARGET_DEAD),
				new StateTransitionValue(_idleState, null, () => true));

			StateMachine.AddStateTransition(
				new StateTransitionKey(_attackState, AttackWizardState.EXIT_REASON_TARGET_OUT_OF_RANGE),
				new StateTransitionValue(_idleState, null, () => true));
		}

		private void OnDestroy()
		{
			_wizard.Damageable.DamageReceived -= OnWizardReceivedDamage;
		}

		private void Update()
		{
			StateMachine.Update();
		}

		private void OnWizardReceivedDamage(object sender, DamageReceivedEventArgs args)
		{
			if (!StateMachine.IsCurrentState(_runAwayState) && !StateMachine.IsCurrentState(_attackState))
			{
				if (Random.Range(0, 2) == 0)
				{
					_runAwayState.Initialize(args.DamageDealer);
					StateMachine.SetCurrentState(_runAwayState);
				}
				else
				{
					_attackState.AttackDamage = 5;
					_attackState.AttackRadius = 2f;
					_attackState.TimeBetweenAttacks = 1f;
					_attackState.Target = args.DamageDealer;
					StateMachine.SetCurrentState(_attackState);
				}
			}
		}
	}
}