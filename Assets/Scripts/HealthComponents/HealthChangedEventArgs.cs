using System;

namespace HealthComponents
{
	public class HealthChangedEventArgs : EventArgs
	{
		public HealthChangedEventArgs(float previousHealth, float currentHealth)
		{
			PreviousHealth = previousHealth;
			CurrentHealth = currentHealth;
		}
		
		public float PreviousHealth { get; }
		public float CurrentHealth { get; }
	}
}