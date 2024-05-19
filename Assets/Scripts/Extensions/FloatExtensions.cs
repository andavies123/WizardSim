using System;

namespace Extensions
{
	/// <summary>
	/// Extension class that contains extensions for float values
	/// </summary>
	public static class FloatExtensions
	{
		/// <summary>
		/// Calculates and returns the percentage. Value/Total * 100
		/// </summary>
		/// <param name="value">The extended value</param>
		/// <param name="total">The total amount to find the percentage from</param>
		/// <returns>Returns a percentage from 0 to 100 (can go over/under)</returns>
		public static float PercentageOf(this float value, float total)
		{
			return PercentageOf01(value, total) * 100;
		}

		/// <summary>
		/// Calculates and returns the percentage from 0 to 1. Value/Total
		/// </summary>
		/// <param name="value">The extended value</param>
		/// <param name="total">The total amount to find the percentage from</param>
		/// <returns>Returns a percentage from 0 to 1 (can go over/under)</returns>
		/// <exception cref="DivideByZeroException">Thrown when total is 0</exception>
		public static float PercentageOf01(this float value, float total)
		{
			if (total == 0)
				throw new DivideByZeroException("Unable to calculate a percentage of 0");
			
			return value / total;
		}
	}
}