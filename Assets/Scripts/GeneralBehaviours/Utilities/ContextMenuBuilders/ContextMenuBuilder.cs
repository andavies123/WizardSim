using System.Text;
using Extensions;
using Game;
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
		private const string HealthPathItem = "Health";
		private const string HealPathItem = "Heal";
		private const string HurtPathItem = "Hurt";

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void AddContextMenuItems()
		{
			AddHealthComponentContextMenuItems();
		}
        
		private static void AddHealthComponentContextMenuItems()
		{
			Globals.ContextMenuInjections.InjectContextMenuOption<HealthComponent>(
				BuildPath(HealthPathItem, HealPathItem, "25%"),
				health => health.IncreaseHealthByPercent(0.25f),
				health => health.IsNotAtMaxHealth(),
				_ => true);
			
			Globals.ContextMenuInjections.InjectContextMenuOption<HealthComponent>(
				BuildPath(HealthPathItem, HurtPathItem, "25%"),
				health => health.DecreaseHealthByPercent(0.25f),
				health => health.IsNotAtMinHealth(),
				_ => true);
			
			Globals.ContextMenuInjections.InjectContextMenuOption<HealthComponent>(
				BuildPath(HealthPathItem, HealPathItem, "100%"),
				health => health.IncreaseHealthByPercent(1f),
				health => health.IsNotAtMaxHealth(),
				_ => true);
			
			Globals.ContextMenuInjections.InjectContextMenuOption<HealthComponent>(
				BuildPath(HealthPathItem, HurtPathItem, "100%"),
				health => health.DecreaseHealthByPercent(1f),
				health => health.IsNotAtMinHealth(),
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