using Game;
using GameWorld.WorldObjects;
using GeneralClasses.Health;
using GeneralClasses.Health.HealthEventArgs;
using GeneralClasses.Health.Interfaces;
using System;
using UnityEngine;

namespace GeneralBehaviours.HealthBehaviours
{
	public class HealthComponent : MonoBehaviour, IHealth
	{
		private IHealth _health = new Health(1);

		public event EventHandler<ReachedMaxHealthEventArgs> ReachedMaxHealth
		{
			add => _health.ReachedMaxHealth += value;
			remove => _health.ReachedMaxHealth -= value;
		}

		public event EventHandler<ReachedMinHealthEventArgs> ReachedMinHealth
		{
			add => _health.ReachedMinHealth += value;
			remove => _health.ReachedMinHealth -= value;
		}

		public event EventHandler<CurrentHealthChangedEventArgs> CurrentHealthChanged
		{
			add => _health.CurrentHealthChanged += value;
			remove => _health.CurrentHealthChanged -= value;
		}

		public event EventHandler<MaxHealthChangedEventArgs> MaxHealthChanged
		{
			add => _health.MaxHealthChanged += value;
			remove => _health.MaxHealthChanged -= value;
		}

		public bool IsAtMaxHealth => _health.IsAtMaxHealth;
		public bool IsAtMinHealth => _health.IsAtMinHealth;

		public float MaxHealth
		{ 
			get => _health.MaxHealth;
			set => _health.MaxHealth = value;
		}

		public float CurrentHealth
		{
			get => _health.CurrentHealth;
			set => _health.CurrentHealth = value;
		}

		public void InitializeWithProperties(HealthProperties healthProperties)
		{
			if (healthProperties == null)
			{
				Debug.LogError($"Unable to initialize {nameof(HealthComponent)} with a null {nameof(healthProperties)}");
				return;
			}

			_health = GlobalFactories.HealthFactory.CreateHealth(healthProperties);
		}

		public void InitializeWithProperties(HealthRelatedProperties healthRelatedProperties)
		{
			InitializeWithProperties(new HealthProperties
			{
				MaxHealth = healthRelatedProperties.MaxHealth,
				CanRechargeHealth = false
			});
		}
		
		internal void IncreaseHealthByPercent(float percent01) => _health.CurrentHealth += _health.MaxHealth * percent01;
		internal void DecreaseHealthByPercent(float percent01) => _health.CurrentHealth -= _health.MaxHealth * percent01;

		internal bool IsNotAtMaxHealth() => !_health.IsAtMaxHealth;
		internal bool IsNotAtMinHealth() => !_health.IsAtMinHealth;
	}
}