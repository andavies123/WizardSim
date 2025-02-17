using System;
using Extensions;
using GameWorld.WorldObjects;
using GameWorld.WorldObjects.Rocks;
using UnityEngine;

namespace GameWorld.Characters.Wizards.States
{
	public class DestroyRockState : WizardState
	{
		public const string EXIT_REASON_ROCK_DESTROYED = nameof(EXIT_REASON_ROCK_DESTROYED);

		private readonly float _timeBetweenAttacks; // How long it takes to attack
		
		private Rock _rock; // The rock that is being targeted
		private float _attackTimer; // The current time since the last attack

		public DestroyRockState(Wizard wizard) : base(wizard)
		{
			_timeBetweenAttacks = 1f / Wizard.WizardStats.RockAttackSpeed.Value;
		}

		public override event EventHandler<string> ExitRequested;

		public override string DisplayName => "Destroy Rock";
		public override string DisplayStatus { get; protected set; } = "Breaking Rock";

		public void Initialize(Rock rock)
		{
			_rock = rock;
			_rock.WorldObject.Destroyed += OnRockDestroyed;
		}
		
		public override void Begin()
		{
			_attackTimer = _timeBetweenAttacks;
			UpdateDisplayStatus();
		}


		public override void Update()
		{
			if (_attackTimer < _timeBetweenAttacks)
			{
				_attackTimer += Time.deltaTime;
				return;
			}

			if (_rock)
			{
				_attackTimer = 0;
				_rock.WorldObject.Damageable.DealDamage(
					Wizard.WizardStats.RockAttackDamage.Value, 
					Wizard.DamageType, 
					Wizard);
				
				// Todo: Update the way a rock gets destroyed
				if (_rock.WorldObject.Health.CurrentHealth <= 0)
					_rock.gameObject.Destroy();

				UpdateDisplayStatus();
			}
			else
			{
				ExitRequested?.Invoke(this, EXIT_REASON_ROCK_DESTROYED);
			}
		}

		public override void End() { }

		private void UpdateDisplayStatus()
		{
			DisplayStatus = $"Breaking Rock - Health: {_rock.WorldObject.Health.CurrentHealth:0.0} / {_rock.WorldObject.Health.MaxHealth:0.0}";
		}

		private void OnRockDestroyed(WorldObject worldObject)
		{
			ExitRequested?.Invoke(this, EXIT_REASON_ROCK_DESTROYED);
		}
	}
}