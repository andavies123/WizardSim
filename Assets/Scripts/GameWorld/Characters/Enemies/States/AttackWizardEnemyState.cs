using System;
using StateMachines;
using GameWorld.Characters.Wizards;
using UnityEngine;
using GameWorld.Characters.States;

namespace GameWorld.Characters.Enemies.States
{
	public class AttackWizardEnemyState : EnemyState
	{
		public const string EXIT_REASON_ATTACK_FINISHED = nameof(EXIT_REASON_ATTACK_FINISHED);

		private readonly StateMachine _stateMachine = new();
		private readonly MoveToPositionCharacterState _moveToState;
		private readonly AttackEnemyState _attackEnemyState;

		public override event EventHandler<string> ExitRequested;

		public AttackWizardEnemyState(Enemy enemy) : base(enemy)
		{
			_moveToState = new MoveToPositionCharacterState(enemy);
			_attackEnemyState = new AttackEnemyState(enemy);

			AddStateTransitions();
		}

		public override string DisplayName => "Attacking";
		public override string DisplayStatus { get; protected set; }

		public float TargetLossRadius { get; set; }
		public float AttackRadius { get; set; }
		public Wizard TargetWizard { get; set; }

		public override void Begin()
		{
			_attackEnemyState.AttackRadius = AttackRadius;
			_attackEnemyState.DamagePerHit = 5;
			_attackEnemyState.SecondsBetweenAttacks = 0.5f;

			_stateMachine.SetCurrentState(_moveToState);
		}

		public override void Update()
		{
			_stateMachine.Update();

			if (TargetWizard)
			{
				if (_stateMachine.IsCurrentState(_moveToState))
				{
					if (Vector3.Distance(Enemy.Position, TargetWizard.Position) >= TargetLossRadius)
					{
						ExitRequested?.Invoke(this, EXIT_REASON_ATTACK_FINISHED);
						return;
					}

					_moveToState.Initialize(TargetWizard.Transform.position, AttackRadius);
				}
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
				new StateTransitionFrom(_moveToState, MoveToPositionCharacterState.EXIT_REASON_ARRIVED_AT_POSITION),
				new StateTransitionTo(_attackEnemyState, InitializeAttackEnemyState, () => true));

			_stateMachine.AddStateTransition(
				new StateTransitionFrom(_attackEnemyState, AttackEnemyState.EXIT_REASON_TARGET_OUT_OF_RANGE),
				new StateTransitionTo(_moveToState, null, () => true));

			_stateMachine.AddStateTransition(
				new StateTransitionFrom(_attackEnemyState, AttackEnemyState.EXIT_REASON_ATTACK_FINISHED),
				new StateTransitionTo(null, OnFinishedAttacking, () => true));
		}

		private void InitializeAttackEnemyState() => _attackEnemyState.Target = TargetWizard;
		private void OnFinishedAttacking() => ExitRequested?.Invoke(this, EXIT_REASON_ATTACK_FINISHED);
	}
}