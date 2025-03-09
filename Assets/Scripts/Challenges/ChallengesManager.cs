using System;
using UnityEngine;

namespace Challenges
{
	public sealed class ChallengesManager : MonoBehaviour
	{
		private readonly DefeatEnemiesChallengeGroup _defeatEnemiesChallengeGroup = new(10);

		public event Action CurrentChallengeUpdated;
		
		public Challenge CurrentChallenge { get; private set; }
		
		public void RefreshChallenge()
		{
			CurrentChallenge = _defeatEnemiesChallengeGroup.GetChallenge();
			CurrentChallengeUpdated?.Invoke();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.C))
			{
				RefreshChallenge();
			}
		}
	}
}