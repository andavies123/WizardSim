using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.ContextMenus
{
	public abstract class ContextMenuUser : MonoBehaviour
	{
		[SerializeField] protected ContextMenuEvents contextMenuEvents;

		protected readonly List<ContextMenuItem> MenuItems = new();
        
		public event Action MenuClosed;

		public IReadOnlyList<ContextMenuItem> AllMenuItems => MenuItems;
		public abstract string MenuTitle { get; }
		public abstract string InfoText { get; protected set; }

		public void OpenMenu()
		{
			contextMenuEvents.RequestMenuOpen(this);
		}

		public void CloseMenu()
		{
			MenuClosed?.Invoke();
		}
	}
}