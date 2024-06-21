using System;

namespace GeneralClasses.Health.HealthEventArgs
{
	public class MaxHealthChangedEventArgs : EventArgs
	{
		public MaxHealthChangedEventArgs(float previousMaxHealth, float newMaxHealth)
		{
			PreviousMaxHealth = previousMaxHealth;
			NewMaxHealth = newMaxHealth;
		}
		
		public float PreviousMaxHealth { get; }
		public float NewMaxHealth { get; }
	}
}