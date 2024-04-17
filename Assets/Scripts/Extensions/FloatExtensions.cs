namespace Extensions
{
	public static class FloatExtensions
	{
		public static float PercentageOf(this float value, float total) => 
			PercentageOf01(value, total) * 100;

		public static float PercentageOf01(this float value, float total) =>
			total != 0 ? value / total : 0;
	}
}