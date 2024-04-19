using System;

namespace GeneralBehaviours.Health
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