using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.ContextMenus
{
	public abstract class ContextMenuUser : MonoBehaviour
	{
		[SerializeField] protected ContextMenuEvents contextMenuEvents;

		protected readonly List<ContextMenuItem> MenuItems = new();
        
		public event Action MenuClosed;

		public IReadOnlyList<ContextMenuItem> AllMenuItems => MenuItems;

		public void OpenMenu()
		{
			contextMenuEvents.RequestMenuOpen(this);
		}

		public void CloseMenu()
		{
			MenuClosed?.Invoke();
		}

		public void UpdateMenuItems()
		{
			MenuItems.ForEach(item => item.RecalculateVisibility());
		}

		protected static bool AlwaysTrue() => true;
		protected static bool AlwaysFalse() => false;
	}
}