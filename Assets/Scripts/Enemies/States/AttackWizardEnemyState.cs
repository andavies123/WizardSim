using System;
using StateMachines;
using Wizards;

namespace Enemies.States
{
	public class AttackWizardEnemyState : EnemyState
	{
		public const string EXIT_REASON_ATTACK_FINISHED = nameof(EXIT_REASON_ATTACK_FINISHED);
		
		private readonly StateMachine _stateMachine = new();
		private readonly EnemyMoveToState _moveToState;
		private readonly AttackEnemyState _attackEnemyState;

		public override event EventHandler<string> ExitRequested;

		public AttackWizardEnemyState(Enemy enemy) : base(enemy)
		{
			_moveToState = new EnemyMoveToState(enemy);
			_attackEnemyState = new AttackEnemyState(enemy);
		}

		public override string DisplayName => "Attacking";
		public override string DisplayStatus { get; protected set; }
		
		public float TargetLossRadius { get; set; }
		public float AttackRadius { get; set; }
		public Wizard TargetWizard { get; set; }

		public override void Begin()
		{
			_moveToState.MaxDistanceForArrival = AttackRadius;
			_attackEnemyState.DamagePerHit = 5;
			_attackEnemyState.SecondsBetweenAttacks = 0.5f;
		}

		public override void Update()
		{
			_stateMachine.Update();
			
			if (TargetWizard)
			{
				// Todo: Check the distance and if its greater than the target loss distance, then finish the state
				
				_moveToState.MoveToPosition = TargetWizard.Transform.position;
			}
			else
			{
				ExitRequested?.Invoke(this, EXIT_REASON_ATTACK_FINISHED);
			}
		}

		public override void End() { }

		private void AddStateTransitions()
		{
			_stateMachine.AddStateTransition(
				new StateTransitionKey(_moveToState, EnemyMoveToState.EXIT_REASON_ARRIVED),
				new StateTransitionValue(_attackEnemyState, InitializeAttackEnemyState, () => true));
			
			_stateMachine.AddStateTransition(
				new StateTransitionKey(_attackEnemyState, AttackEnemyState.EXIT_REASON_ATTACK_FINISHED),
				new StateTransitionValue(null, OnFinishedAttacking, () => true));
		}
		
		private void InitializeAttackEnemyState() => _attackEnemyState.Target = TargetWizard.Health;
		private void OnFinishedAttacking() => ExitRequested?.Invoke(this, EXIT_REASON_ATTACK_FINISHED);
	}
}