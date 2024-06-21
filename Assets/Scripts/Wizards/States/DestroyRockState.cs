using System;
using Extensions;
using GameWorld.WorldObjects.Rocks;
using UnityEngine;

namespace Wizards.States
{
	public class DestroyRockState : WizardState
	{
		private Rock _rock;
		
		public DestroyRockState(Wizard wizard) : base(wizard) { }

		public event EventHandler RockDestroyed;

		public override string DisplayName => "Destroy Rock";
		public override string DisplayStatus { get; protected set; } = "Breaking Rock";

		public void Initialize(Rock rock)
		{
			_rock = rock;
			_rock.Destroyed += OnRockDestroyed;
		}
		
		public override void Begin()
		{
			if (!_rock)
				ForceExit(StateExitEventArgs.NotInitialized);

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
				_rock.Health.CurrentHealth -= 0.5f;
				
				// Todo: Update the way a rock gets destroyed
				if (_rock.Health.CurrentHealth <= 0)
					_rock.gameObject.Destroy();

				UpdateDisplayStatus();
			}
		}

		public override void End() { }

		private void UpdateDisplayStatus()
		{
			DisplayStatus = $"Breaking Rock - Health: {_rock.Health.CurrentHealth:0.0} / {_rock.Health.MaxHealth:0.0}";
		}

		private void OnRockDestroyed(object sender, EventArgs args)
		{
			RockDestroyed?.Invoke(this, EventArgs.Empty);
		}
	}
}