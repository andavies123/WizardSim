using System;

namespace Challenges
{
	/// <summary>
	/// Challenges give the player an opportunity to earn better upgrades or even an extra upgrade
	/// at the end of the game cycle.
	///
	/// Challenges must be completed in order to get the rewards
	/// </summary>
	public sealed class Challenge
	{
		/// <summary>
		/// THe display name of this challenge
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// A description of the challenge to help give more details
		/// on how the player can complete it
		/// </summary>
		public string Description { get; set; }
		
		/// <summary>
		/// Logic to check what the current progress for this challenge is
		/// </summary>
		/// <returns>
		/// Can be a string that displays the current progress.
		/// Can be null if a challenge doesn't require special tracking
		/// </returns>
		public Func<string> ProgressText { get; set; }
		
		/// <summary>
		/// Logic to check whether or not the challenge is completed.
		/// </summary>
		/// <returns>
		/// True: Challenge is completed
		/// False: Challenge is not completed
		/// </returns>
		public Func<bool> CompletionCriteria { get; set; }
	}
}