using System.Text;
using Extensions;
using Game;
using GeneralBehaviours.HealthBehaviours;
using GeneralClasses.Health;
using UI.ContextMenus;
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
        
		/// <summary>
		/// Adds health related context menu items to the passed context menu user.
		/// Using extension method to make it cleaner to type/read
		/// </summary>
		/// <param name="contextMenuUser">The extended context menu user that will have context menu items added</param>
		/// <param name="healthComponent">The health component that will be used to heal/hurt</param>
		public static void AddHealthComponentContextMenuItems(this ContextMenuUser contextMenuUser, HealthComponent healthComponent)
		{
			if (!contextMenuUser || !healthComponent)
			{
				Debug.LogWarning($"Unable to add health context menu items. {nameof(contextMenuUser)} or {nameof(healthComponent)} is null");
				return;
			}
			
			// Globals.ContextMenuInjections.InjectContextMenuOption<Health>(
			// 	BuildPath(HealthPathItem, HealPathItem, "25%"),
			// 	health => healthComponent.IncreaseHealthByPercent(0.25f),
			// 	healthComponent.IsNotAtMaxHealth,
			// 	() => true);
			//
			// Globals.ContextMenuInjections.InjectContextMenuOption<Health>(
			// 	BuildPath(HealthPathItem, HurtPathItem, "25%"),
			// 	health => healthComponent.DecreaseHealthByPercent(0.25f),
			// 	healthComponent.IsNotAtMinHealth,
			// 	() => true);
			//
			// Globals.ContextMenuInjections.InjectContextMenuOption<Health>(
			// 	BuildPath(HealthPathItem, HealPathItem, "100%"),
			// 	health => healthComponent.IncreaseHealthByPercent(1f),
			// 	healthComponent.IsNotAtMaxHealth,
			// 	() => true);
			//
			// Globals.ContextMenuInjections.InjectContextMenuOption<Health>(
			// 	BuildPath(HealthPathItem, HurtPathItem, "100%"),
			// 	health => healthComponent.DecreaseHealthByPercent(1f),
			// 	healthComponent.IsNotAtMinHealth,
			// 	() => true);
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