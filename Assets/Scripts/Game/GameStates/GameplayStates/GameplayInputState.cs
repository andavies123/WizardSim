using System;
using CameraComponents;
using Game.MessengerSystem;
using InputStates.InputEventArgs;
using UI;
using UI.ContextMenus;
using UI.InfoWindows;
using UI.Messages;
using UnityEngine.InputSystem;

namespace Game.GameStates.GameplayStates
{
	public class GameplayInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private readonly InteractableRaycaster _interactableRaycaster;
		
		private PlayerInputActions.GameplayActions _gameplay;
		
		public GameplayInputState(InteractableRaycaster interactableRaycaster)
		{
			_interactableRaycaster = interactableRaycaster;
			_gameplay = _playerInputActions.Gameplay;
		}
		
		public event EventHandler PauseInputPerformed;
		public event EventHandler<Interactable> OpenInfoWindowRequested;
		public event EventHandler<ContextMenuUser> OpenContextMenuRequested;
		public event EventHandler CloseInfoWindowRequested;
		public event EventHandler CloseContextMenuRequested;
		public event EventHandler OpenTaskManagementRequested;
		
		public bool ShowInteractions => true;

		public void Enable()
		{
			_gameplay.PauseGame.performed += OnPauseActionPerformed;
			_gameplay.OpenTaskManagement.performed += OnOpenTaskManagementActionPerformed;
			_gameplay.Cancel.performed += OnCancelActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary += OnInteractablePrimaryActionSelected;
			_interactableRaycaster.InteractableSelectedSecondary += OnInteractableSelectedSecondary;
			_interactableRaycaster.NonInteractableSelectedPrimary += OnNonInteractablePrimaryActionSelected;

			_gameplay.Enable();
		}

		public void Disable()
		{
			_gameplay.Disable();
			
			_gameplay.PauseGame.performed -= OnPauseActionPerformed;
			_gameplay.OpenTaskManagement.performed -= OnOpenTaskManagementActionPerformed;
			_gameplay.Cancel.performed -= OnCancelActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary -= OnInteractablePrimaryActionSelected;
			_interactableRaycaster.InteractableSelectedSecondary -= OnInteractableSelectedSecondary;
			_interactableRaycaster.NonInteractableSelectedPrimary -= OnNonInteractablePrimaryActionSelected;
			
			GlobalMessenger.Unsubscribe<OpenContextMenuRequest>(OnOpenContextMenuRequestReceived);
		}
		
		private void OnPauseActionPerformed(InputAction.CallbackContext callbackContext)
		{
			PauseInputPerformed?.Invoke(this, EventArgs.Empty);
		}

		private void OnCancelActionPerformed(InputAction.CallbackContext callbackContext)
		{
			CloseInfoWindowRequested?.Invoke(this, EventArgs.Empty);
		}

		private void OnInteractablePrimaryActionSelected(object sender, InteractableRaycasterEventArgs args)
		{
			CloseContextMenuRequested?.Invoke(this, EventArgs.Empty);
			OpenInfoWindowRequested?.Invoke(this, args.Interactable);
		}
		
		private void OnInteractableSelectedSecondary(object sender, InteractableRaycasterEventArgs args)
		{
			if (!args?.Interactable)
				return;
			
			if (args.Interactable.TryGetComponent(out ContextMenuUser contextMenuUser))
				OpenContextMenuRequested?.Invoke(this, contextMenuUser);
		}

		private void OnNonInteractablePrimaryActionSelected(object sender, EventArgs args)
		{
			if (!UIState.IsMouseOverGameObject)
			{
				CloseInfoWindowRequested?.Invoke(this, EventArgs.Empty);
				CloseContextMenuRequested?.Invoke(this, EventArgs.Empty);
			}
		}

		private void OnOpenContextMenuRequestReceived(OpenContextMenuRequest message)
		{
			OpenInfoWindowRequested?.Invoke(this, message.ContextMenuUser.Interactable);
		}

		private void OnOpenTaskManagementActionPerformed(InputAction.CallbackContext callbackContext)
		{
			OpenTaskManagementRequested?.Invoke(this, EventArgs.Empty);
		}
	}
}