using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenuUser : MonoBehaviour
	{
		[SerializeField] protected ContextMenuEvents contextMenuEvents;

		private readonly List<ContextMenuItem> _menuItems = new();
        
		public event Action MenuClosed;

		public IReadOnlyList<ContextMenuItem> AllMenuItems => _menuItems;

		public void AddMenuItem(ContextMenuItem menuItem) => _menuItems.Add(menuItem);
		public void UpdateMenuItems() => _menuItems.ForEach(item => item.RecalculateVisibility());
		
		public void OpenMenu() => contextMenuEvents.RequestMenuOpen(this);
		public void CloseMenu() => MenuClosed?.Invoke();
	}
}