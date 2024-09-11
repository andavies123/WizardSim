using Game;
using GameWorld.WorldObjects;
using GeneralClasses.Health;
using GeneralClasses.Health.Interfaces;
using UnityEngine;

namespace GeneralBehaviours.HealthBehaviours
{
	public class HealthComponent : MonoBehaviour
	{
		public IHealth Health { get; private set; }
		
		public void InitializeWithProperties(HealthProperties healthProperties)
		{
			if (healthProperties == null)
			{
				Debug.Log($"Unable to initialize {nameof(HealthComponent)} with a null {nameof(healthProperties)}");
				Destroy(this);
				return;
			}
			
			Health = GlobalFactories.HealthFactory.CreateHealth(healthProperties);
		}

		public void InitializeWithProperties(HealthRelatedProperties healthRelatedProperties)
		{
			InitializeWithProperties(new HealthProperties
			{
				MaxHealth = healthRelatedProperties.MaxHealth,
				CanRechargeHealth = false
			});
		}
		
		internal void IncreaseHealthByPercent(float percent01) => Health.CurrentHealth += Health.MaxHealth * percent01;
		internal void DecreaseHealthByPercent(float percent01) => Health.CurrentHealth -= Health.MaxHealth * percent01;

		internal bool IsNotAtMaxHealth() => !Health.IsAtMaxHealth;
		internal bool IsNotAtMinHealth() => !Health.IsAtMinHealth;
	}
}