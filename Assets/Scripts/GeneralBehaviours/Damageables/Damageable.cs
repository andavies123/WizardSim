using System;
using DamageTypes;
using Game;
using GameWorld.Characters;
using GeneralBehaviours.HealthBehaviours;
using UI.ContextMenus;
using UnityEngine;

namespace GeneralBehaviours.Damageables
{
	[RequireComponent(typeof(HealthComponent))]
	public class Damageable : MonoBehaviour, IContextMenuUser
	{
		public HealthComponent Health { get; private set; }

		/// <summary>
		/// Raised when this object received damage greater than 0
		/// </summary>
		public event EventHandler<DamageReceivedEventArgs> DamageReceived;
		public event EventHandler<DamageableDestroyedEventArgs> Destroyed;

		public void DealDamage(float damageAmount, DamageType? damageType, Character damageDealer)
		{
			lock (Health)
			{
				if (Health.IsAtMinHealth || damageAmount <= 0f)
				{
					// We don't need to progress further if any of the above cases are true
					return;
				}

				// Cache the health before hand so we can calculate the literal damage that was dealt
				float beforeHealth = Health.CurrentHealth;
				Health.CurrentHealth -= damageAmount;
				float damageReceived = beforeHealth - Health.CurrentHealth;

				// Raise any necessary events
				if (damageReceived > 0)
				{
					Globals.DamageTextManager.ShowDamageText(transform.position + new Vector3(.5f, 2.5f, .5f), damageType, damageReceived);
					DamageReceived?.Invoke(this, new DamageReceivedEventArgs(damageDealer, damageReceived));
				}

				if (Health.IsAtMinHealth)
				{
					Destroyed?.Invoke(this, new DamageableDestroyedEventArgs(damageDealer));
				}
			}
		}

		private void Awake()
		{
			Health = GetComponent<HealthComponent>();
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

	public class DamageableDestroyedEventArgs : EventArgs
	{
		public Character Destroyer { get; }

		public DamageableDestroyedEventArgs(Character destroyer)
		{
			Destroyer = destroyer;
		}
	}
}