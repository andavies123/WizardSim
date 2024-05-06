using System;
using UnityEngine.InputSystem;

namespace InputStates
{
	public class SecondaryGameplayInput : IInput
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private PlayerInputActions.SecondaryGameplayActions _secondaryGameplay;
		
		public SecondaryGameplayInput()
		{
			_secondaryGameplay = _playerInputActions.SecondaryGameplay;
		}
		
		public event EventHandler PauseActionPerformed;
		
		public bool ShowInteractions => false;

		public void Enable()
		{
			_secondaryGameplay.PauseGame.performed += OnPauseActionPerformed;

			_secondaryGameplay.Enable();
		}

		public void Disable()
		{
			_secondaryGameplay.Disable();
			
			_secondaryGameplay.PauseGame.performed += OnPauseActionPerformed;
		}
		
		private void OnPauseActionPerformed(InputAction.CallbackContext callbackContext) => PauseActionPerformed?.Invoke(this, EventArgs.Empty);
	}
}