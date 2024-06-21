using System;

namespace GeneralClasses.Health.HealthEventArgs
{
	public class CurrentHealthChangedEventArgs : EventArgs
	{
		public CurrentHealthChangedEventArgs(float previousCurrentCurrentHealth, float newCurrentHealth)
		{
			PreviousCurrentHealth = previousCurrentCurrentHealth;
			NewCurrentHealth = newCurrentHealth;
		}
		
		public float PreviousCurrentHealth { get; }
		public float NewCurrentHealth { get; }

		public bool IsIncrease => NewCurrentHealth > PreviousCurrentHealth;
		public bool IsDecrease => NewCurrentHealth < PreviousCurrentHealth;
	}
}