using System.Text;
using Extensions;
using Game;
using GeneralBehaviours.Damageables;
using GeneralBehaviours.HealthBehaviours;
using UnityEngine;

namespace GeneralBehaviours.Utilities.ContextMenuBuilders
{
	/// <summary>
	/// This class was made to remove the following dependencies between
	/// ContextMenuUser and other components
	/// </summary>
	public static class ContextMenuBuilder
	{
		private const string HealthGroupName = "Health";
		private const string DamageableGroupName = "Damageable";
		
		private const string HealPathItem = "Heal";
		private const string HurtPathItem = "Hurt";

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void AddContextMenuItems()
		{
			if (Globals.Instance == null)
				return;
			
			AddHealthComponentContextMenuItems();
			AddDamageableComponentContextMenuItems();
		}
        
		private static void AddHealthComponentContextMenuItems()
		{
			Globals.ContextMenuInjections.InjectContextMenuOption<HealthComponent>(
				BuildPath(HealthGroupName, HealPathItem, "25%"),
				health => health.IncreaseHealthByPercent(0.25f),
				health => health.IsNotAtMaxHealth(),
				_ => true);
			
			Globals.ContextMenuInjections.InjectContextMenuOption<HealthComponent>(
				BuildPath(HealthGroupName, HurtPathItem, "25%"),
				health => health.DecreaseHealthByPercent(0.25f),
				health => health.IsNotAtMinHealth(),
				_ => true);
			
			Globals.ContextMenuInjections.InjectContextMenuOption<HealthComponent>(
				BuildPath(HealthGroupName, HealPathItem, "100%"),
				health => health.IncreaseHealthByPercent(1f),
				health => health.IsNotAtMaxHealth(),
				_ => true);
			
			Globals.ContextMenuInjections.InjectContextMenuOption<HealthComponent>(
				BuildPath(HealthGroupName, HurtPathItem, "100%"),
				health => health.DecreaseHealthByPercent(1f),
				health => health.IsNotAtMinHealth(),
				_ => true);
		}
		
		private static void AddDamageableComponentContextMenuItems()
		{
			Globals.ContextMenuInjections.InjectContextMenuOption<Damageable>(
				BuildPath(DamageableGroupName, HealPathItem, "25%"),
				damageable => damageable.DealDamage(-damageable.Health.MaxHealth/4, null, null),
				damageable => damageable.Health.IsNotAtMaxHealth(),
				_ => true);
			
			Globals.ContextMenuInjections.InjectContextMenuOption<Damageable>(
				BuildPath(DamageableGroupName, HurtPathItem, "25%"),
				damageable => damageable.DealDamage(damageable.Health.MaxHealth/4, null, null),
				damageable => damageable.Health.IsNotAtMinHealth(),
				_ => true);
			
			Globals.ContextMenuInjections.InjectContextMenuOption<Damageable>(
				BuildPath(DamageableGroupName, HealPathItem, "100%"),
				damageable => damageable.DealDamage(-damageable.Health.MaxHealth, null, null),
				damageable => damageable.Health.IsNotAtMaxHealth(),
				_ => true);
			
			Globals.ContextMenuInjections.InjectContextMenuOption<Damageable>(
				BuildPath(DamageableGroupName, HurtPathItem, "100%"),
				damageable => damageable.DealDamage(damageable.Health.MaxHealth, null, null),
				damageable => damageable.Health.IsNotAtMinHealth(),
				_ => true);
		}

		/// <summary>
		/// Builds a valid path string for a context menu item
		/// </summary>
		/// <param name="pathItems">The collection of path items that will be used to build the path. Order does matter</param>
		/// <returns>A valid path string containing a separator character between each path item</returns>
		public static string BuildPath(params string[] pathItems)
		{
			StringBuilder pathStringBuilder = new();

			for (int pathIndex = 0; pathIndex < pathItems.Length; pathIndex++)
			{
				string pathItem = pathItems[pathIndex];
				
				if (pathItem.IsNullOrWhiteSpace())
					continue;

				pathStringBuilder.Append(pathItem);

				if (pathIndex < pathItems.Length - 1)
					pathStringBuilder.Append('|');
			}
			
			return pathStringBuilder.ToString();
		}
	}
}