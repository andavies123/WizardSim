using GameWorld.Characters;
using System;
using UnityEngine;

namespace Wizards.States
{
	public class AttackWizardState : WizardState
	{
		public const string EXIT_REASON_TARGET_DEAD = nameof(EXIT_REASON_TARGET_DEAD);
		public const string EXIT_REASON_TARGET_OUT_OF_RANGE = nameof(EXIT_REASON_TARGET_OUT_OF_RANGE);

		private float _attackTimer = 0f;

		public override event EventHandler<string> ExitRequested;

		public AttackWizardState(Wizard wizard) : base(wizard) { }

		public override string DisplayName => "Attacking";
		public override string DisplayStatus { get; protected set; }

		public Character Target { get; set; }
		public float AttackRadius { get; set; }
		public float TimeBetweenAttacks { get; set; }
		public float AttackDamage { get; set; }

		private bool CanAttack => _attackTimer >= TimeBetweenAttacks;

		public void Initialize(Character target)
		{
			Target = target;
		}

		public override void Begin() 
		{
			// Able to do their first attack right away
			_attackTimer = TimeBetweenAttacks;
		}

		public override void End() { }

		public override void Update() 
		{
			if (!Target)
			{
				ExitRequested?.Invoke(this, EXIT_REASON_TARGET_DEAD);
				return;
			}

			if (Vector3.Distance(Wizard.Position, Target.Position) > AttackRadius)
			{
				ExitRequested?.Invoke(this, EXIT_REASON_TARGET_OUT_OF_RANGE);
				return;
			}

			_attackTimer += Time.deltaTime;

			if (CanAttack)
			{
				Target.Damageable.DealDamage(AttackDamage, Wizard);
				_attackTimer -= TimeBetweenAttacks;
			}
		}
	}
}