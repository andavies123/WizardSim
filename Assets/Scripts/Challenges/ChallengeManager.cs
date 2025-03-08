using UnityEngine;

namespace Challenges
{
	public sealed class ChallengeManager : MonoBehaviour
	{
		private readonly KillEnemiesChallengeGroup _killEnemiesChallengeGroup = new(10);
		
		public Challenge GetRandomChallenge()
		{
			return _killEnemiesChallengeGroup.GetChallenge();
		}
	}
}