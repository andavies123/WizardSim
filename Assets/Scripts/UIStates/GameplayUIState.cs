using System;
using Extensions;
using UI;
using UI.ContextMenus;
using UI.HotBarUI;
using UI.InfoWindows;
using UI.TaskManagement;
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
		[SerializeField] private TaskManagementUI taskManagementWindow;
		
		public event EventHandler PauseButtonPressed;

		public ContextMenu ContextMenu => contextMenu;
        
		public void OpenInfoWindow(Interactable interactable) => infoWindow.OpenWindow(interactable);
		public void CloseInfoWindow() => infoWindow.CloseWindow();
		
		public void OpenContextMenu(ContextMenuUser contextMenuUser, Vector3 screenPosition) => contextMenu.OpenMenu(contextMenuUser, screenPosition);
		public void CloseContextMenu() => contextMenu.CloseMenu();

		public void OpenTaskManagementWindow() => taskManagementWindow.Open();
		public void CloseTaskManagementWindow() => taskManagementWindow.Close();

		protected override void Awake()
		{
			base.Awake();
			pauseButton.ThrowIfNull(nameof(pauseButton));
			infoWindow.ThrowIfNull(nameof(infoWindow));
			contextMenu.ThrowIfNull(nameof(contextMenu));
			hotBar.ThrowIfNull(nameof(hotBar));
			taskManagementWindow.ThrowIfNull(nameof(taskManagementWindow));
		}
		
        protected override void OnStateEnabled()
		{
			pauseButton.onClick.AddListener(OnPauseButtonPressed);

			contextMenu.MenuClosed += OnContextMenuClosed;

			hotBar.enabled = true;
		}

		protected override void OnStateDisabled()
		{
			pauseButton.onClick.RemoveListener(OnPauseButtonPressed);

			contextMenu.MenuClosed -= OnContextMenuClosed;

			hotBar.enabled = false;
		}

		private void OnPauseButtonPressed() => PauseButtonPressed?.Invoke(this, EventArgs.Empty);
		private void OnContextMenuClosed(object sender, EventArgs args) => CloseInfoWindow();
	}
}