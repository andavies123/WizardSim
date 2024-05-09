using System;
using CameraComponents;
using UIStates;
using UnityEngine.InputSystem;

namespace InputStates
{
	public class SecondaryGameplayInput : IInput
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private readonly InteractableRaycaster _interactableRaycaster;
		private readonly GameplayUIState _gameplayUIState;
		
		private PlayerInputActions.SecondaryGameplayActions _secondaryGameplay;
		
		public SecondaryGameplayInput(InteractableRaycaster interactableRaycaster, GameplayUIState gameplayUIState)
		{
			_interactableRaycaster = interactableRaycaster;
			_gameplayUIState = gameplayUIState;
			_secondaryGameplay = _playerInputActions.SecondaryGameplay;
		}
		
		public event EventHandler PauseActionPerformed;
		public event EventHandler<InteractableRaycasterEventArgs> OpenInfoWindowRequested;
		public event EventHandler CloseInfoWindowRequested;
		public event EventHandler CloseContextMenuRequested;
		
		public bool ShowInteractions => false;

		public void Enable()
		{
			_secondaryGameplay.PauseGame.performed += OnPauseActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary += OnInteractablePrimaryActionSelected;
			_interactableRaycaster.NonInteractableSelectedPrimary += OnNonInteractablePrimaryActionSelected;

			_secondaryGameplay.Enable();
		}

		public void Disable()
		{
			_secondaryGameplay.Disable();
			
			_secondaryGameplay.PauseGame.performed += OnPauseActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary -= OnInteractablePrimaryActionSelected;
			_interactableRaycaster.NonInteractableSelectedPrimary -= OnNonInteractablePrimaryActionSelected;
		}
		
		private void OnPauseActionPerformed(InputAction.CallbackContext callbackContext) => PauseActionPerformed?.Invoke(this, EventArgs.Empty);

		private void OnInteractablePrimaryActionSelected(object sender, InteractableRaycasterEventArgs args)
		{
			
			CloseContextMenuRequested?.Invoke(this, EventArgs.Empty);
			OpenInfoWindowRequested?.Invoke(this, new InteractableRaycasterEventArgs(args.Interactable));
		}

		private void OnNonInteractablePrimaryActionSelected(object sender, EventArgs args)
		{
			if (!_gameplayUIState.IsMouseOverGameObject)
			{
				CloseInfoWindowRequested?.Invoke(this, EventArgs.Empty);
				CloseContextMenuRequested?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}