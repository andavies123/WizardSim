using System;
using Extensions;
using GameWorld.WorldObjects;
using GameWorld.WorldObjects.Rocks;
using GameWorld.WorldResources;
using UnityEngine;

namespace GameWorld.Characters.Wizards.States
{
	public class DestroyRockState : WizardState
	{
		public const string EXIT_REASON_ROCK_DESTROYED = nameof(EXIT_REASON_ROCK_DESTROYED);
		
		private Rock _rock;
		
		public DestroyRockState(Wizard wizard) : base(wizard) { }

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
			_attackTimer = 0.0f;
			UpdateDisplayStatus();
		}

		private const float TimeBetweenAttacks = 0.1f;
		private float _attackTimer = 0.0f;

		public override void Update()
		{
			if (_attackTimer < TimeBetweenAttacks)
			{
				_attackTimer += Time.deltaTime;
				return;
			}

			if (_rock)
			{
				_attackTimer = 0;
				_rock.WorldObject.Damageable.DealDamage(0.5f, Wizard);
				
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