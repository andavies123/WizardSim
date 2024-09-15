namespace GameWorld.Characters.Wizards
{
	public class Age : IAge
	{
		private const int SecondsInYear = 31536000;
		private const int SecondsInDay = 86400;

		public float Years => Seconds / SecondsInYear;
		public float Days => Seconds / SecondsInDay;
		public float Seconds { get; private set; }

		public void IncreaseAge(float elapsedWorldTimeSeconds)
		{
			Seconds += elapsedWorldTimeSeconds;
		}
	}
}