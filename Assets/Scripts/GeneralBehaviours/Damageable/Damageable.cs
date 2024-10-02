using GameWorld.Characters;
using GeneralBehaviours.HealthBehaviours;
using System;
using UnityEngine;

namespace GeneralBehaviours.Damageable
{
	[RequireComponent(typeof(HealthComponent))]
	public class Damageable : MonoBehaviour
	{
		private HealthComponent _health;

		/// <summary>
		/// Raised when this object received damage greater than 0
		/// </summary>
		public event EventHandler<DamageReceivedEventArgs> DamageReceived;

		public void DealDamage(float damageAmount, Character damageDealer)
		{
			if (!_health)
			{
				Debug.LogError("Health component has not been initialized yet");
				return;
			}

			// Make sure we aren't dealing negative damage
			damageAmount = Mathf.Max(damageAmount, 0);

			// Cache the health before hand so we can calculate the literal damage that was dealt
			float beforeHealth = _health.CurrentHealth;
			_health.CurrentHealth -= damageAmount;

			// Let subscribers know that damage was received if any was actually dealt
			float damageReceived = beforeHealth - _health.CurrentHealth;
			if (damageReceived > 0)
			{
				DamageReceived?.Invoke(this, new DamageReceivedEventArgs(damageDealer, damageReceived));
			}
		}

		private void Awake()
		{
			_health = GetComponent<HealthComponent>();
		}
	}

	public class DamageReceivedEventArgs : EventArgs
	{
		/// <summary>
		/// The character object that is dealing the damage
		/// </summary>
		public Character DamageDealer { get; }

		/// <summary>
		/// The amount of damage that was dealt
		/// </summary>
		public float DamageReceived { get; }

		public DamageReceivedEventArgs(Character damageDealer, float damageReceived)
		{
			DamageDealer = damageDealer;
			DamageReceived = damageReceived;
		}
	}
}
