using System;
using UnityEngine;

namespace Wizards.States
{
	public class WizardWaitState : WizardState
	{
		private float _currentWaitTime = 0f;

		public WizardWaitState(Wizard wizard) : base(wizard) { }

		public event Action WaitFinished;

		public override string DisplayName => "Waiting";
		public override string DisplayStatus { get; protected set; }

		public float WaitTime { get; set; }

		public override void Begin()
		{
			ResetWaitTimes();
		}

		public override void Update()
		{
			_currentWaitTime += Time.deltaTime;

			if (_currentWaitTime > WaitTime)
				WaitFinished?.Invoke();
			else
				DisplayStatus = $"{_currentWaitTime / WaitTime * 100:0.0}%";
		}
		
		public override void End() { }

		private void ResetWaitTimes()
		{
			_currentWaitTime = 0f;
		}
	}
}