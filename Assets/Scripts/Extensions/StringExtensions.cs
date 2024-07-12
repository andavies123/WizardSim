namespace Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Extension method for string.IsNullOrWhiteSpace(string)
		/// </summary>
		/// <param name="str">The extended string</param>
		/// <returns>True if <see cref="str"/> is either null or contains only whitespace characters
		/// like a new line or tab or spaces</returns>
		public static bool IsNullOrWhiteSpace(this string str)
		{
			return string.IsNullOrWhiteSpace(str);
		}

		/// <summary>
		/// Extension method for string.IsNullOrEmpty(string)
		/// </summary>
		/// <param name="str">The extended string</param>
		/// <returns>True if <see cref="str"/> is either null or has a length of 0</returns>
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}
	}
}