namespace GameWorld.Characters.Wizards
{
	public interface IAge
	{
		float Years { get; }
		float Days { get; }
		float Seconds { get; }

		void IncreaseAge(float elapsedWorldTime);
	}
}