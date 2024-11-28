using System;
using UI;
using UnityEngine.InputSystem;

namespace Game.GameStates.GameplayStates
{
	public class GameplayInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();

		private PlayerInputActions.GameplayActions _gameplay;
		
		public GameplayInputState()
		{
			_gameplay = _playerInputActions.Gameplay;
		}
		
		public event EventHandler PauseInputPerformed;
		public event EventHandler<Interactable> OpenInfoWindowRequested;
		public event EventHandler CloseInfoWindowRequested;
		public event EventHandler OpenTaskManagementRequested;
		
		public bool ShowInteractions => true;

		public void Enable()
		{
			_gameplay.PauseGame.performed += OnPauseActionPerformed;
			_gameplay.OpenTaskManagement.performed += OnOpenTaskManagementActionPerformed;
			_gameplay.Cancel.performed += OnCancelActionPerformed;
			
			_gameplay.Enable();
		}

		public void Disable()
		{
			_gameplay.Disable();
			
			_gameplay.PauseGame.performed -= OnPauseActionPerformed;
			_gameplay.OpenTaskManagement.performed -= OnOpenTaskManagementActionPerformed;
			_gameplay.Cancel.performed -= OnCancelActionPerformed;
		}
		
		private void OnPauseActionPerformed(InputAction.CallbackContext callbackContext)
		{
			PauseInputPerformed?.Invoke(this, EventArgs.Empty);
		}

		private void OnCancelActionPerformed(InputAction.CallbackContext callbackContext)
		{
			CloseInfoWindowRequested?.Invoke(this, EventArgs.Empty);
		}

		private void OnOpenTaskManagementActionPerformed(InputAction.CallbackContext callbackContext)
		{
			OpenTaskManagementRequested?.Invoke(this, EventArgs.Empty);
		}
	}
}