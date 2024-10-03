using System;
using UnityEngine;

namespace GameWorld.Characters.States
{
	public class WaitCharacterState : CharacterState
	{
		public const string EXIT_REASON_DONE_WAITING = nameof(EXIT_REASON_DONE_WAITING);

		private float _currentWaitTime = 0f;
		
		public override event EventHandler<string> ExitRequested;

		public WaitCharacterState(Character character) : base(character) { }
		
		public override string DisplayName => "Waiting";
		public override string DisplayStatus { get; protected set; }

		public float WaitTime { get; set; } = -1;

		public override void Begin()
		{
			ResetWaitTimes();
		}

		public override void Update()
		{
			// Unlimited wait time
			if (WaitTime == -1f)
			{
				DisplayStatus = "Waiting Indefinitely";
				return;
			}

			_currentWaitTime += Time.deltaTime;

			if (_currentWaitTime > WaitTime)
				ExitRequested?.Invoke(this, EXIT_REASON_DONE_WAITING);
			else
				DisplayStatus = $"{_currentWaitTime / WaitTime * 100:0.0}%";
		}
		
		public override void End() { }
		
		private void ResetWaitTimes() => _currentWaitTime = 0f;
	}
}