using System;
using UI;
using UI.ContextMenus;
using UI.HotBarUI;
using UI.InfoWindows;
using UnityEngine;
using UnityEngine.UI;
using ContextMenu = UI.ContextMenus.ContextMenu;

namespace UIStates
{
	public class GameplayUIState : UIState
	{
		[Header("UI Elements")]
		[SerializeField] private Button pauseButton;
		[SerializeField] private InfoWindow infoWindow;
		[SerializeField] private ContextMenu contextMenu;
		[SerializeField] private HotBar hotBar;
		
		public event EventHandler PauseButtonPressed;

		public void OpenInfoWindow(Interactable interactable) => infoWindow.OpenWindow(interactable);
		public void CloseInfoWindow() => infoWindow.CloseWindow();
		
		public void OpenContextMenu(ContextMenuUser contextMenuUser) => contextMenu.OpenMenu(contextMenuUser);
		public void CloseContextMenu() => contextMenu.CloseMenu();

		protected override void OnStateEnabled()
		{
			if (pauseButton)
				pauseButton.onClick.AddListener(OnPauseButtonPressed);

			ContextMenuUser.RequestMenuOpen += OnContextMenuOpenRequested;

			if (contextMenu)
				contextMenu.MenuClosed += OnContextMenuClosed;

			if (hotBar)
				hotBar.enabled = true;
		}

		protected override void OnStateDisabled()
		{
			if (pauseButton)
				pauseButton.onClick.RemoveListener(OnPauseButtonPressed);

			ContextMenuUser.RequestMenuOpen -= OnContextMenuOpenRequested;

			if (contextMenu)
				contextMenu.MenuClosed -= OnContextMenuClosed;

			if (hotBar)
				hotBar.enabled = false;
		}

		private void OnPauseButtonPressed() => PauseButtonPressed?.Invoke(this, EventArgs.Empty);
		private void OnContextMenuClosed(object sender, EventArgs args) => CloseInfoWindow();

		private void OnContextMenuOpenRequested(object sender, ContextMenuUserEventArgs args)
		{
			OpenContextMenu(args.ContextMenuUser);
			OpenInfoWindow(args.ContextMenuUser.Interactable);
		}

	}
}