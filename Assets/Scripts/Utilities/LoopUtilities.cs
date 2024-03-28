using System;

namespace Utilities
{
	public static class LoopUtilities
	{
		/// <summary>
		/// Shorthand method that uses an internal for loop
		/// to loop through a given action a specified number of times
		/// </summary>
		/// <param name="loopCount">The number of times the action will be called</param>
		/// <param name="action">The action that will be called repeatedly</param>
		public static void Loop(int loopCount, Action action)
		{
			if (loopCount <= 0)
				return;

			for (int i = 0; i < loopCount; i++)
				action?.Invoke();
		}

		/// <summary>
		/// Shorthand method that uses an internal for loop
		/// to loop through a given action a specified number of times
		/// and passes the loop index to the function
		/// </summary>
		/// <param name="loopCount">The number of times the action will be called</param>
		/// <param name="action">The action that will be called repeatedly</param>
		public static void LoopWithIndex(int loopCount, Action<int> action)
		{
			if (loopCount <= 0)
				return;
			
			for (int i = 0; i < loopCount; i++)
				action?.Invoke(i);
		}
	}
}