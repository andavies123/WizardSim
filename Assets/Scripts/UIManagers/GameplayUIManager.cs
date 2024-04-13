using System;
using CameraComponents;
using UI;
using UI.ContextMenus;
using UI.InfoWindows;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ContextMenu = UI.ContextMenus.ContextMenu;

namespace UIManagers
{
	public class GameplayUIManager : UIManager
	{
		[Header("UI Elements")]
		[SerializeField] private Button pauseButton;
		
		[Header("Info Window")]
		[SerializeField] private InfoWindow infoWindow;
		
		[Header("Context Menu")]
		[SerializeField] private ContextMenu contextMenu;
		[SerializeField] private ContextMenuEvents contextMenuEvents;

		[Header("General Components")]
		[SerializeField] private InteractableRaycaster interactableRaycaster;
		
		public event Action PauseButtonPressed;
		
		protected override void Awake()
		{
			base.Awake();
			
			if (pauseButton)
				pauseButton.onClick.AddListener(OnPauseButtonPressed);

			if (contextMenu && contextMenuEvents)
				contextMenuEvents.ContextMenuOpenRequested += OnContextMenuOpenRequested;

			if (contextMenu)
				contextMenu.MenuClosed += OnContextMenuClosed;
			
			if (interactableRaycaster)
				interactableRaycaster.InteractableSelectedPrimary += OnInteractableSelectedPrimary;
		}

		protected void Update()
		{
			if (contextMenu.IsOpen || infoWindow.IsOpen)
			{
				if (Input.GetMouseButtonDown(0) && 
				    !EventSystem.current.IsPointerOverGameObject() &&
				    !interactableRaycaster.IsInteractableCurrentlyHovered)
				{
					contextMenu.CloseMenu();
					infoWindow.CloseWindow();
				}
			}
		}

		protected void OnDestroy()
		{
			if (pauseButton)
				pauseButton.onClick.RemoveListener(OnPauseButtonPressed);

			if (contextMenuEvents)
				contextMenuEvents.ContextMenuOpenRequested -= OnContextMenuOpenRequested;

			if (contextMenu)
				contextMenu.MenuClosed -= OnContextMenuClosed;

			if (interactableRaycaster)
				interactableRaycaster.InteractableSelectedPrimary -= OnInteractableSelectedPrimary;
		}

		private void OnPauseButtonPressed() => PauseButtonPressed?.Invoke();

		private void OnContextMenuOpenRequested(ContextMenuUser user)
		{
			contextMenu.OpenMenu(user);
			infoWindow.OpenWindow(user.GetComponent<Interactable>());
		}

		private void OnContextMenuClosed() => infoWindow.CloseWindow();
		private void OnInteractableSelectedPrimary(Interactable interactable) => infoWindow.OpenWindow(interactable);
	}
}