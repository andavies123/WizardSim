using System;

namespace HealthComponents
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