using UnityEngine;

namespace Challenges
{
	public class DefeatEnemiesChallengeGroup : ChallengeGroup
	{
		public DefeatEnemiesChallengeGroup(float selectionWeight) : base(selectionWeight) { }

		public override Challenge GetChallenge()
		{
			int enemiesToKill = Random.Range(10, 101);
			const int enemiesKilled = 20;
			return new Challenge
			{
				Name = $"Defeat {enemiesToKill} Enemies",
				Description = "This challenge will be completed once the settlement collectively defeats the required amount of enemies",
				ProgressText = () => $"({enemiesKilled}/{enemiesToKill}) defeated",
				CompletionCriteria = () => enemiesKilled >= enemiesToKill
			};
		}
	}
}