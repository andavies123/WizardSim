using System;

namespace GeneralClasses.Health.HealthEventArgs
{
	public class ReachedMaxHealthEventArgs : EventArgs
	{
		public ReachedMaxHealthEventArgs(float currentHealth)
		{
			CurrentHealth = currentHealth;
		}
		
		public float CurrentHealth { get; }
	}
}