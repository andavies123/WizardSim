using UnityEngine;

namespace Game.Values
{
	public static class GameValues
	{
		public static TimeValues Time { get; private set; } = new();
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			Time = new TimeValues();
		}
	}
}