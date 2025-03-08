using System;

namespace Challenges
{
	public abstract class ChallengeGroup
	{
		protected ChallengeGroup(float selectionWeight)
		{
			if (selectionWeight < 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(selectionWeight),
					selectionWeight, 
					"Value should be a non-negative number");
			}

			SelectionWeight = selectionWeight;
		}
		
		/// <summary>
		/// The weight that this group has against other groups to be selected
		/// to supply a challenge
		/// </summary>
		public float SelectionWeight { get; }

		/// <summary>
		/// Gets a challenge from this group
		/// </summary>
		/// <returns>A challenge related to this group</returns>
		public abstract Challenge GetChallenge();
	}
}