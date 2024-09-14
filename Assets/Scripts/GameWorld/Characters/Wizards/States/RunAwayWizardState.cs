using System;
using UnityEngine;

namespace GameWorld.Characters.Wizards.States
{
	public class RunAwayWizardState : WizardState
	{
		public const string EXIT_REASON_GOT_AWAY = nameof(EXIT_REASON_GOT_AWAY);

		private float _currentTimeRunningAway = 0f;

		public override event EventHandler<string> ExitRequested;

		public override string DisplayName => "Running Away";
		public override string DisplayStatus { get; protected set; }
		public float RunAwayTime { get; set; } = 10f;

		public RunAwayWizardState(Wizard wizard, float runAwayTime) : base(wizard)
		{
			RunAwayTime = runAwayTime;
		}

		public override void Begin()
		{
			// Temporary position for now to run away to
			Wizard.Movement.SetMoveToPosition(Vector3.zero, 1f);
			_currentTimeRunningAway = 0f;
		}

		public override void Update()
		{
			_currentTimeRunningAway += Time.deltaTime;

			if (_currentTimeRunningAway >= RunAwayTime)
			{
				ExitRequested?.Invoke(this, EXIT_REASON_GOT_AWAY);
			}
		}

		public override void End() { }
	}
}