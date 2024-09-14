namespace GameWorld.Characters.Wizards
{
	public interface IAge
	{
		float CurrentAge { get; }

		void IncreaseAge(float elapsedWorldTime);
	}
}