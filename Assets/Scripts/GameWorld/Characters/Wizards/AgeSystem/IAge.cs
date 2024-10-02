namespace GameWorld.Characters.Wizards.AgeSystem
{
	public interface IAge
	{
		/// <summary>
		/// How many game years this object has lived.
		/// Use this value when referring to this objects age
		/// </summary>
		float Years { get; }
		
		/// <summary>
		/// The total number of game days this object has lived.
		/// This number will not loop around back to zero
		/// </summary>
		float Days { get; }
		
		/// <summary>
		/// The total number of game seconds this object has lived.
		/// This number does not loop back to zero
		/// </summary>
		float Seconds { get; }

		/// <summary>
		/// Use this to increase the age of this object.
		/// </summary>
		/// <param name="elapsedWorldTime">How many seconds this object's age should increase by</param>
		void IncreaseAge(float elapsedWorldTime);
	}
}