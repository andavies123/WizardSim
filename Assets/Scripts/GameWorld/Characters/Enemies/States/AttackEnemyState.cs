﻿using System;
using DamageTypes;
using Game;
using UnityEngine;
using Time = UnityEngine.Time;

namespace GameWorld.Characters.Enemies.States
{
	public class AttackEnemyState : EnemyState
	{
		public const string EXIT_REASON_ATTACK_FINISHED = nameof(EXIT_REASON_ATTACK_FINISHED);
		public const string EXIT_REASON_TARGET_OUT_OF_RANGE = nameof(EXIT_REASON_TARGET_OUT_OF_RANGE);

		private float _secondsSinceLastAttack = 0f;
		private DamageType _attackDamageType;

		public override event EventHandler<string> ExitRequested;

		public AttackEnemyState(Enemy enemy) : base(enemy) { }

		public override string DisplayName => "Attacking";
		public override string DisplayStatus { get; protected set; }

		public Character Target { get; set; }
		public float AttackRadius { get; set; }
		public float DamagePerHit { get; set; }
		public float SecondsBetweenAttacks { get; set; }

		public override void Begin()
		{
			// They should be able to attack as soon as they reach the target
			_secondsSinceLastAttack = SecondsBetweenAttacks;
			_attackDamageType = Dependencies.Get<ResourceRepo<DamageType>>().Repo["Fire"];
		}

		public override void Update()
		{
			// Make sure the target still exists
			if (!Target)
			{
				ExitRequested?.Invoke(this, EXIT_REASON_ATTACK_FINISHED);
				return;
			}

			// Make sure the target hasn't gone too far away
			if (Vector3.Distance(Target.Position, Enemy.Position) >= AttackRadius)
			{
				ExitRequested?.Invoke(this, EXIT_REASON_TARGET_OUT_OF_RANGE);
				return;
			}

			// Check to see if we can deal some damage yet
			if (_secondsSinceLastAttack < SecondsBetweenAttacks)
			{
				_secondsSinceLastAttack += Time.deltaTime;
				return;
			}

			// Not setting attack timer back to zero.
			// Depending on the timing, we could lose a lot of attacks over time
			_secondsSinceLastAttack -= SecondsBetweenAttacks;
			Target.Damageable.DealDamage(DamagePerHit, _attackDamageType, Enemy);

			if (Target.Health.IsAtMinHealth)
			{
				ExitRequested?.Invoke(this, EXIT_REASON_ATTACK_FINISHED);
			}
		}

		public override void End() { }
	}
}