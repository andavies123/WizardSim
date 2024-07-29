namespace Utilities
{
	/// <summary>
	/// Static utility class to hold constants related to time conversions
	/// </summary>
	public static class TimeConstants
	{
		public const int SECONDS_PER_MINUTE = 60;
		public const int SECONDS_PER_HOUR = 3600;
		public const int SECONDS_PER_DAY = 86400;
		
		public const int MINUTES_PER_HOUR = 60;
		public const int MINUTES_PER_DAY = 1440;
		
		public const int HOURS_PER_DAY = 24;

		public const int DAYS_PER_YEAR = 365;

		public const int MONTHS_PER_YEAR = 12;

		public const int YEARS_PER_DECADE = 10;
		public const int YEARS_PER_CENTURY = 100;

		public static readonly string[] MonthNames =
		{
			"January", "February", "March", "April", "May", "June",
			"July", "August", "September", "October", "November", "December"
		};
	}
}