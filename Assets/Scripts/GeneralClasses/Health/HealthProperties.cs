using System;

namespace GeneralClasses.Health
{
	[Serializable]
	public class HealthProperties
	{
		public int MaxHealth { get; set; }
		
		// Recharge Properties
		public bool CanRechargeHealth { get; set; } = false;
		public float HealthGainedPerInterval { get; set; } = 1;
		public int RechargeIntervalMSec { get; set; } = 1000;
		public int TimeUntilRechargeStartsMSec { get; set; } = 10000;
	}
}