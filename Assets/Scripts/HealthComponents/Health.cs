using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HealthComponents
{
	public class Health : MonoBehaviour
	{
		private const int MinHealth = 0;
		
		[SerializeField, Min(0.1f)] private float maxHealth = 10;

		private float _currentHealth;

		public event EventHandler<ReachedMaxHealthEventArgs> ReachedMaxHealth;
		public event EventHandler<ReachedMinHealthEventArgs> ReachedMinHealth;
		public event EventHandler<HealthChangedEventArgs> CurrentHealthChanged; 
		
		public float MaxHealth => maxHealth;
		public bool IsAtMaxHealth => Mathf.Approximately(CurrentHealth, MaxHealth);

		public float CurrentHealth
		{
			get => _currentHealth;
			private set
			{
				float previousValue = _currentHealth;
				_currentHealth = Mathf.Clamp(value, MinHealth, MaxHealth);

				if (!Mathf.Approximately(previousValue, _currentHealth))
					OnCurrentHealthChanged(previousValue, _currentHealth);
				
				if (Mathf.Approximately(_currentHealth, MinHealth))
					OnReachedMinHealth(_currentHealth);
				
				if (Mathf.Approximately(_currentHealth, MaxHealth))
					OnReachedMaxHealth(_currentHealth);
			}
		}

		public void IncreaseHealth(float amount) => CurrentHealth += amount;
		public void DecreaseHealth(float amount) => CurrentHealth -= amount;

		private void Awake() => CurrentHealth = Random.Range(MinHealth, MaxHealth);

		private void OnReachedMaxHealth(float currentHealth) =>
			ReachedMaxHealth?.Invoke(this, new ReachedMaxHealthEventArgs(currentHealth));

		private void OnReachedMinHealth(float currentHealth) =>
			ReachedMinHealth?.Invoke(this, new ReachedMinHealthEventArgs(currentHealth));

		private void OnCurrentHealthChanged(float previousHealth, float currentHealth) =>
			CurrentHealthChanged?.Invoke(this, new HealthChangedEventArgs(previousHealth, currentHealth));
	}
}