using System;
using Extensions;
using UnityEngine;
using ContextMenu = UI.ContextMenus.ContextMenu;

namespace Game.GameStates.ContextMenuStates
{
	public class ContextMenuUIState : UIState
	{
		[SerializeField] private ContextMenu contextMenu;

		public event EventHandler ContextMenuClosed;
		
		public ContextMenu ContextMenu => contextMenu;
		
		protected override void Awake()
		{
			base.Awake();

			contextMenu.ThrowIfNull(nameof(contextMenu));
		}
        
		protected override void OnStateEnabled()
		{
			ContextMenu.MenuClosed += OnContextMenuClosed;
		}

		protected override void OnStateDisabled()
		{
			ContextMenu.MenuClosed -= OnContextMenuClosed;
		}

		private void OnContextMenuClosed(object sender, EventArgs args)
		{
			ContextMenuClosed?.Invoke(sender, args);
		}
	}
}