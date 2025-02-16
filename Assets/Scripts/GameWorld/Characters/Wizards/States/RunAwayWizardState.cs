using System;
using GeneralBehaviours.Damageables;
using UnityEngine;

namespace GameWorld.Characters.Wizards.States
{
	public class RunAwayWizardState : WizardState
	{
		public const string EXIT_REASON_GOT_AWAY = nameof(EXIT_REASON_GOT_AWAY);

		private Character _attacker;
		private float _currentTimeRunningAway = 0f;

		public override event EventHandler<string> ExitRequested;

		public override string DisplayName => "Running Away";
		public override string DisplayStatus { get; protected set; }
		public float RunAwayTime { get; set; } = 10f;

		public RunAwayWizardState(Wizard wizard, float runAwayTime) : base(wizard)
		{
			RunAwayTime = runAwayTime;
		}

		public void Initialize(Character attacker)
		{
			_attacker = attacker;
		}

		public override void Begin()
		{
			if (_attacker)
				Wizard.Movement.SetMoveDirection(Wizard.Position - _attacker.Position);
			else
				Wizard.Movement.SetMoveToPosition(Vector3.zero, 1f);

			_currentTimeRunningAway = 0f;

			Wizard.Damageable.DamageReceived += OnDamageReceived;
		}

		public override void End() 
		{
			Wizard.Damageable.DamageReceived -= OnDamageReceived;
		}

		public override void Update()
		{
			_currentTimeRunningAway += Time.deltaTime;

			if (_currentTimeRunningAway >= RunAwayTime)
			{
				ExitRequested?.Invoke(this, EXIT_REASON_GOT_AWAY);
			}
		}

		private void OnDamageReceived(object sender, DamageReceivedEventArgs args)
		{
			// We want to reset the runing away time so the wizard doesn't
			// randomly stop running while in the middle of being attacked
			_currentTimeRunningAway = 0;
		}
	}
}