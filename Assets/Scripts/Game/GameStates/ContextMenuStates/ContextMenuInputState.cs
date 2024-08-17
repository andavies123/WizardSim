using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.InputSystem.PlayerInputActions;

namespace Game.GameStates.ContextMenuStates
{
	public class ContextMenuInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private ContextMenuActions _contextMenuActions;
		
		public ContextMenuInputState()
		{
			_contextMenuActions = _playerInputActions.ContextMenu;
		}

		public event EventHandler<ContextMenuNavigationEventArgs> NavigationActionPerformed;
		public event EventHandler SelectActionPerformed;
		public event EventHandler CloseActionPerformed;
		
		public bool ShowInteractions => true;
		
		public void Enable()
		{
			_contextMenuActions.Navigation.performed += OnNavigationActionPerformed;
			_contextMenuActions.Select.performed += OnSelectActionPerformed;
			_contextMenuActions.Close.performed += OnCloseActionPerformed;
			
			_contextMenuActions.Enable();
		}

		public void Disable()
		{
			_contextMenuActions.Disable();
			
			_contextMenuActions.Navigation.performed -= OnNavigationActionPerformed;
			_contextMenuActions.Select.performed -= OnSelectActionPerformed;
			_contextMenuActions.Close.performed -= OnCloseActionPerformed;
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

		private void OnSelectActionPerformed(CallbackContext context) => SelectActionPerformed?.Invoke(this, EventArgs.Empty);
		private void OnCloseActionPerformed(CallbackContext context) => CloseActionPerformed?.Invoke(this, EventArgs.Empty);
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