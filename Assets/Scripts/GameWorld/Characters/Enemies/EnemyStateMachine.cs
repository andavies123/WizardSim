using GameWorld.Characters.Enemies.States;
using GeneralBehaviours;
using StateMachines;
using UnityEngine;
using GameWorld.Characters.Wizards;

namespace GameWorld.Characters.Enemies
{
	[RequireComponent(typeof(Enemy))]
	public class EnemyStateMachine : StateMachineComponent
	{
		[Header("Idle State Defaults")]
		[SerializeField] private float idleRadius = 10;

		[Header("Attack State Defaults")]
		[SerializeField] private float targetRadius = 5f;
		[SerializeField] private float targetLossRadius = 10f;
		[SerializeField] private float attackRadius = 1f;

		private Enemy _enemy;

		private IdleEnemyState _idleState;
		private AttackWizardEnemyState _attackWizardState;

		public void Idle()
		{
			StateMachine.SetCurrentState(_idleState);
		}

		public void AttackWizard(Wizard targetWizard)
		{
			_attackWizardState.TargetWizard = targetWizard;

			StateMachine.SetCurrentState(_attackWizardState);
		}

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();
		}

		private void Start()
		{
			InitializeStates();
			InitializeStateTransitions();

			Idle();
		}

		private void Update()
		{
			StateMachine.Update();

			if (!StateMachine.IsCurrentState(_attackWizardState) && CheckForSurroundingWizards(out Wizard targetWizard))
				AttackWizard(targetWizard);
		}

		private void InitializeStates()
		{
			_idleState = new IdleEnemyState(_enemy) { IdleRadius = idleRadius };

			_attackWizardState = new AttackWizardEnemyState(_enemy)
			{
				AttackRadius = attackRadius,
				TargetLossRadius = targetLossRadius
			};
		}

		private void InitializeStateTransitions()
		{
			StateMachine.AddStateTransition(
				new StateTransitionKey(_attackWizardState, AttackWizardEnemyState.EXIT_REASON_ATTACK_FINISHED),
				new StateTransitionValue(_idleState, null, () => true));
		}

		private bool CheckForSurroundingWizards(out Wizard targetWizard)
		{
			// Get the surrounding wizard
			if (!_enemy.ParentWorld.WizardManager.TryGetClosestWizard(_enemy.transform.position, out targetWizard, out float distance))
				return false;

			// Check the distance
			if (distance > targetRadius)
				return false;

			return true;
		}
	}
}