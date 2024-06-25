using GeneralBehaviours.HealthBehaviours;
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
            
			contextMenuUser.AddMenuItem(new ContextMenuItem(
				"Heal 25%", 
				() => healthComponent.IncreaseHealthByPercent(0.25f), 
				healthComponent.IsNotAtMaxHealth));
			
			contextMenuUser.AddMenuItem(new ContextMenuItem(
				"Hurt 25%", 
				() => healthComponent.DecreaseHealthByPercent(0.25f), 
				healthComponent.IsNotAtMinHealth));
			
			contextMenuUser.AddMenuItem(new ContextMenuItem(
				"Heal 100%", 
				() => healthComponent.IncreaseHealthByPercent(1), 
				healthComponent.IsNotAtMaxHealth));
			
			contextMenuUser.AddMenuItem(new ContextMenuItem(
				"Hurt 100%", 
				() => healthComponent.DecreaseHealthByPercent(1), 
				healthComponent.IsNotAtMinHealth));
		}
	}
}