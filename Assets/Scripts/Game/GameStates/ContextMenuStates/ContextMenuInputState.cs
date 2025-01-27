using System;
using CameraComponents;
using Extensions;
using UI.ContextMenus;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.InputSystem.PlayerInputActions;

namespace Game.GameStates.ContextMenuStates
{
	public class ContextMenuInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private readonly InteractableRaycaster _interactableRaycaster;
		private ContextMenuActions _contextMenuActions;
		
		public ContextMenuInputState(InteractableRaycaster interactableRaycaster)
		{
			_interactableRaycaster = interactableRaycaster.ThrowIfNull(nameof(interactableRaycaster));
			_contextMenuActions = _playerInputActions.ContextMenu;
		}

		public event EventHandler<ContextMenuNavigationEventArgs> NavigationActionPerformed;
		public event EventHandler<ContextMenuUser> OpenNewContextMenuRequested;
		public event EventHandler SelectActionPerformed;
		public event EventHandler CloseRequested;
		
		public bool ShowInteractions => true;
		
		public void Enable()
		{
			_interactableRaycaster.InteractableSelectedPrimary += OnInteractableSelectedPrimary;
			_interactableRaycaster.InteractableSelectedSecondary += OnInteractableSelectedSecondary;
			_interactableRaycaster.NonInteractableSelectedPrimary += OnNonInteractableSelected;
			_interactableRaycaster.NonInteractableSelectedSecondary += OnNonInteractableSelected;
			
			_contextMenuActions.Navigation.performed += OnNavigationActionPerformed;
			_contextMenuActions.Select.performed += OnSelectActionPerformed;
			_contextMenuActions.Close.performed += OnCloseActionPerformed;
			
			_contextMenuActions.Enable();
		}

		public void Disable()
		{
			_interactableRaycaster.InteractableSelectedPrimary -= OnInteractableSelectedPrimary;
			_interactableRaycaster.InteractableSelectedSecondary -= OnInteractableSelectedSecondary;
			_interactableRaycaster.NonInteractableSelectedPrimary -= OnNonInteractableSelected;
			_interactableRaycaster.NonInteractableSelectedSecondary -= OnNonInteractableSelected;
			
			_contextMenuActions.Disable();
			
			_contextMenuActions.Navigation.performed -= OnNavigationActionPerformed;
			_contextMenuActions.Select.performed -= OnSelectActionPerformed;
			_contextMenuActions.Close.performed -= OnCloseActionPerformed;
		}

		private void OnInteractableSelectedSecondary(object sender, InteractableRaycasterEventArgs args)
		{
			if (!args?.Interactable)
				return;

			if (args.Interactable.TryGetComponent(out ContextMenuUser contextMenuUser))
				OpenNewContextMenuRequested?.Invoke(this, contextMenuUser);
		}

		private void OnNavigationActionPerformed(CallbackContext context)
		{
			float value = context.ReadValue<float>();
			NavigationOption navigationOption;
			
			if (value > 0) navigationOption = NavigationOption.Down;
			else if (value < 0) navigationOption = NavigationOption.Up;
			else return; // No input
			
			NavigationActionPerformed?.Invoke(this, new ContextMenuNavigationEventArgs(navigationOption));
		}


		private void OnInteractableSelectedPrimary(object sender, InteractableRaycasterEventArgs args) => CloseRequested?.Invoke(this, EventArgs.Empty);
		private void OnNonInteractableSelected(object sender, EventArgs args) => CloseRequested?.Invoke(this, EventArgs.Empty);
		private void OnSelectActionPerformed(CallbackContext context) => SelectActionPerformed?.Invoke(this, EventArgs.Empty);
		private void OnCloseActionPerformed(CallbackContext context) => CloseRequested?.Invoke(this, EventArgs.Empty);
	}

	public class ContextMenuNavigationEventArgs : EventArgs
	{
		public ContextMenuNavigationEventArgs(NavigationOption navigationOption)
		{
			NavigationOption = navigationOption;
		}
		
		public NavigationOption NavigationOption { get; }
	}
	
	public enum NavigationOption
	{
		Up,
		Down,
		Left,
		Right
	}
}