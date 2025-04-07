using UnityEngine;

namespace Game.Values
{
	public static class GameValues
	{
		public static StockpileValues Stockpile { get; private set; } = new();
		public static TimeValues Time { get; private set; } = new();
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			Stockpile = new StockpileValues();
			Time = new TimeValues();
		}
	}
}