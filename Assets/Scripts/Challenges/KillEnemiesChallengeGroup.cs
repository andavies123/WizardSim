using UnityEngine;

namespace Challenges
{
	public class KillEnemiesChallengeGroup : ChallengeGroup
	{
		public KillEnemiesChallengeGroup(float selectionWeight) : base(selectionWeight) { }

		public override Challenge GetChallenge()
		{
			int enemiesToKill = Random.Range(10, 100);
			const int enemiesKilled = 5;
			return new Challenge
			{
				Name = $"Kill {enemiesToKill} Enemies",
				Description = "This challenge will be completed once the settlement collectively kills the required amount of enemies",
				ProgressTracker = () => $"({enemiesKilled}/{enemiesToKill})",
				CompletionCriteria = () => enemiesKilled >= enemiesToKill
			};
		}
	}
}