namespace Utilities
{
	public static class ShaderUtilities
	{
		/// <summary>
		/// Converts a bool value into a float value corresponding to the bool value
		/// </summary>
		/// <param name="value">The bool value that will be converted into a float</param>
		/// <returns>True = 1 | False = 0</returns>
		public static float BoolToShaderFloat(bool value) => value ? 1 : 0;
	}
}