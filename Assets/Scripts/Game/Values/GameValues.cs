using UnityEngine;

namespace Game.Values
{
	public static class GameValues
	{
		public static SettingsValues Settings { get; private set; } = new();
		public static SettlementValues Settlement { get; private set; } = new();
		public static StockpileValues Stockpile { get; private set; } = new();
		public static TimeValues Time { get; private set; } = new();
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			Settings = new SettingsValues();
			Settlement = new SettlementValues();
			Stockpile = new StockpileValues();
			Time = new TimeValues();
		}
	}
}