using System;

namespace GeneralClasses.Health.HealthEventArgs
{
	public class ReachedMinHealthEventArgs : EventArgs
	{
		public ReachedMinHealthEventArgs(float currentHealth)
		{
			CurrentHealth = currentHealth;
		}

		public float CurrentHealth { get; }
	}
}