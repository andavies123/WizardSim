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

			_destroyTimer = 0.0f;
		}

		private const float TimeToDestroy = 5f;
		private float _destroyTimer = 0.0f;

		public override void Update()
		{
			if (_destroyTimer < TimeToDestroy)
			{
				_destroyTimer += Time.deltaTime;
				return;
			}

			// Todo: Update the way a rock takes damage / is destroyed
			if (_rock)
			{
				_rock.gameObject.Destroy();
			}
		}

		public override void End() { }

		private void OnRockDestroyed(object sender, EventArgs args) => RockDestroyed?.Invoke(this, EventArgs.Empty);
	}
}