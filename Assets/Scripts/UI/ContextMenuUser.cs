using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
	public abstract class ContextMenuUser : MonoBehaviour
	{
		[SerializeField] protected ContextMenuEvents contextMenuEvents;

		public event Action MenuClosed;
		
		public abstract IReadOnlyList<ContextMenuItem> AllMenuItems { get; }
		public abstract string MenuTitle { get; }
		
		public abstract void OpenMenu();

		public void CloseMenu()
		{
			MenuClosed?.Invoke();
		}
	}
	
	public abstract class ContextMenuUser<T> : ContextMenuUser where T : ContextMenuItem
	{
		protected readonly List<T> MenuItems = new();

		public override IReadOnlyList<ContextMenuItem> AllMenuItems => MenuItems.Cast<ContextMenuItem>().ToList();

		public override void OpenMenu()
		{
			contextMenuEvents.RequestMenuOpen(this);
		}
	}
}