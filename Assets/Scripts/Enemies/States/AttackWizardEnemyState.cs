using System;
using GeneralBehaviours.HealthBehaviours;
using StateMachines;
using UnityEngine;
using Wizards;

namespace Enemies.States
{
	public class AttackWizardEnemyState : EnemyState
	{
		private readonly StateMachine _stateMachine = new();
		private readonly EnemyMoveToState _moveToState;
		private readonly AttackEnemyState _attackEnemyState;

		public event EventHandler AttackFinished;

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
			
			_moveToState.ArrivedAtPosition += OnArrivedAtTarget;
			_attackEnemyState.AttackingFinished += OnAttackingFinished;
			
			MoveToWizard(TargetWizard.Transform.position);
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
				AttackFinished?.Invoke(this, EventArgs.Empty);
			}
		}

		public override void End()
		{
			_moveToState.ArrivedAtPosition -= OnArrivedAtTarget;
			_attackEnemyState.AttackingFinished -= OnAttackingFinished;
		}

		private void MoveToWizard(Vector3 wizardPosition)
		{
			_moveToState.MoveToPosition = wizardPosition;
			_stateMachine.SetCurrentState(_moveToState);
		}

		private void AttackWizard(HealthComponent wizardHealth)
		{
			_attackEnemyState.Target = wizardHealth;
			_stateMachine.SetCurrentState(_attackEnemyState);
		}

		private void OnArrivedAtTarget(object sender, EventArgs args) => AttackWizard(TargetWizard.Health);
		private void OnAttackingFinished(object sender, EventArgs args) => AttackFinished?.Invoke(this, EventArgs.Empty);
	}
}