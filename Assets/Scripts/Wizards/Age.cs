namespace Wizards
{
	public class Age : IAge
	{
		public float CurrentAge { get; private set; }

		public void IncreaseAge(float elapsedWorldTime)
		{
			CurrentAge += elapsedWorldTime;
		}
	}
}