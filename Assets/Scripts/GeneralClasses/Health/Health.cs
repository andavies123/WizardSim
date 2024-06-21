using System;
using GeneralClasses.Health.HealthEventArgs;
using GeneralClasses.Health.Interfaces;
using UnityEngine;

namespace GeneralClasses.Health
{
	public class Health : IHealth
	{
		private const int MinHealth = 0;

		private float _currentHealth;
		private float _maxHealth;

		public Health(float maxHealth)
		{
			MaxHealth = maxHealth;
			CurrentHealth = MaxHealth;
		}

		public Health(HealthProperties healthProperties) : this(healthProperties.MaxHealth) { }

		public event EventHandler<ReachedMaxHealthEventArgs> ReachedMaxHealth;
		public event EventHandler<ReachedMinHealthEventArgs> ReachedMinHealth;
		public event EventHandler<CurrentHealthChangedEventArgs> CurrentHealthChanged;
		public event EventHandler<MaxHealthChangedEventArgs> MaxHealthChanged;
		
		public bool IsAtMaxHealth => Mathf.Approximately(CurrentHealth, MaxHealth);
		public bool IsAtMinHealth => Mathf.Approximately(CurrentHealth, MinHealth);

		public float MaxHealth
		{
			get => _maxHealth;
			set
			{
				float previousMaxHealth = _maxHealth;
				_maxHealth = MathF.Max(MinHealth, value); // Max Health shouldn't be less than Min Health
				
				if (!Mathf.Approximately(previousMaxHealth, _maxHealth))
					OnMaxHealthChanged(previousMaxHealth, _maxHealth);
			}
		}

		public float CurrentHealth
		{
			get => _currentHealth;
			set
			{
				float previousCurrentHealth = _currentHealth;
				_currentHealth = Mathf.Clamp(value, MinHealth, MaxHealth);

				if (!Mathf.Approximately(previousCurrentHealth, _currentHealth))
					OnCurrentHealthChanged(previousCurrentHealth, _currentHealth);
				
				if (Mathf.Approximately(_currentHealth, MinHealth))
					OnReachedMinHealth(_currentHealth);
				
				if (Mathf.Approximately(_currentHealth, MaxHealth))
					OnReachedMaxHealth(_currentHealth);
			}
		}
		
		#region Event Invokers
		
		private void OnReachedMaxHealth(float currentHealth)
		{
			ReachedMaxHealth?.Invoke(this, new ReachedMaxHealthEventArgs(currentHealth));
		}

		private void OnReachedMinHealth(float currentHealth)
		{
			ReachedMinHealth?.Invoke(this, new ReachedMinHealthEventArgs(currentHealth));
		}

		private void OnCurrentHealthChanged(float previousHealth, float currentHealth)
		{
			CurrentHealthChanged?.Invoke(this, new CurrentHealthChangedEventArgs(previousHealth, currentHealth));
		}

		private void OnMaxHealthChanged(float previousMaxHealth, float currentMaxHealth)
		{
			MaxHealthChanged?.Invoke(this, new MaxHealthChangedEventArgs(previousMaxHealth, currentMaxHealth));
		}
		
		#endregion
	}
}