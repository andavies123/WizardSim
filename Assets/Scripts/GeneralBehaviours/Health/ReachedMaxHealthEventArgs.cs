using System;

namespace GeneralBehaviours.Health
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