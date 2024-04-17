using System;

namespace HealthComponents
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