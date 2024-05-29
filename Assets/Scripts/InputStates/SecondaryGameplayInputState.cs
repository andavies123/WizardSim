using System;
using CameraComponents;
using Game.MessengerSystem;
using InputStates.InputEventArgs;
using UI.Messages;
using UIStates;
using UnityEngine.InputSystem;

namespace InputStates
{
	public class SecondaryGameplayInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private readonly InteractableRaycaster _interactableRaycaster;
		private readonly GameplayUIState _gameplayUIState;
		
		private PlayerInputActions.SecondaryGameplayActions _secondaryGameplay;
		
		public SecondaryGameplayInputState(InteractableRaycaster interactableRaycaster, GameplayUIState gameplayUIState)
		{
			_interactableRaycaster = interactableRaycaster;
			_gameplayUIState = gameplayUIState;
			_secondaryGameplay = _playerInputActions.SecondaryGameplay;
		}
		
		public event EventHandler PauseActionPerformed;
		public event EventHandler<OpenInfoWindowEventArgs> OpenInfoWindowRequested;
		public event EventHandler<OpenContextMenuEventArgs> OpenContextMenuRequested;
		public event EventHandler CloseInfoWindowRequested;
		public event EventHandler CloseContextMenuRequested;
		public event EventHandler OpenTaskManagementRequested;
		
		public bool ShowInteractions => false;

		public void Enable()
		{
			_secondaryGameplay.PauseGame.performed += OnPauseActionPerformed;
			_secondaryGameplay.OpenTaskManagement.performed += OnOpenTaskManagementActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary += OnInteractablePrimaryActionSelected;
			_interactableRaycaster.NonInteractableSelectedPrimary += OnNonInteractablePrimaryActionSelected;
			
			GlobalMessenger.Subscribe<OpenContextMenuRequest>(OnOpenContextMenuRequestReceived);

			_secondaryGameplay.Enable();
		}

		public void Disable()
		{
			_secondaryGameplay.Disable();
			
			_secondaryGameplay.PauseGame.performed -= OnPauseActionPerformed;
			_secondaryGameplay.OpenTaskManagement.performed -= OnOpenTaskManagementActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary -= OnInteractablePrimaryActionSelected;
			_interactableRaycaster.NonInteractableSelectedPrimary -= OnNonInteractablePrimaryActionSelected;
			
			GlobalMessenger.Unsubscribe<OpenContextMenuRequest>(OnOpenContextMenuRequestReceived);
		}
		
		private void OnPauseActionPerformed(InputAction.CallbackContext callbackContext) => PauseActionPerformed?.Invoke(this, EventArgs.Empty);

		private void OnInteractablePrimaryActionSelected(object sender, InteractableRaycasterEventArgs args)
		{
			CloseContextMenuRequested?.Invoke(this, EventArgs.Empty);
			OpenInfoWindowRequested?.Invoke(this, new OpenInfoWindowEventArgs(args.Interactable));
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
			// TODO: UPDATE THE CONTEXT MENU USERS TO BE UPDATED FROM HERE, THE CLICKS STILL SHOW UP EVEN WHEN CONTEXT MENU DOESN'T OPEN
			OpenContextMenuRequested?.Invoke(this, new OpenContextMenuEventArgs(message.ContextMenuUser));
			OpenInfoWindowRequested?.Invoke(this, new OpenInfoWindowEventArgs(message.ContextMenuUser.Interactable));
		}

		private void OnOpenTaskManagementActionPerformed(InputAction.CallbackContext callbackContext)
		{
			OpenTaskManagementRequested?.Invoke(this, EventArgs.Empty);
		}
	}
}