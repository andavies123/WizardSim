﻿using System;
using UnityEngine.InputSystem;

namespace InputStates
{
	public class PauseMenuInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		
		public event Action ResumeActionPerformed;

		public bool ShowInteractions => false;

		public void Enable()
		{
			_playerInputActions.PauseMenu.ResumeGame.performed += OnResumeActionPerformed;
			
			_playerInputActions.PauseMenu.Enable();
		}

		public void Disable()
		{	
			_playerInputActions.PauseMenu.Disable();

			_playerInputActions.PauseMenu.ResumeGame.performed -= OnResumeActionPerformed;
		}
		
		private void OnResumeActionPerformed(InputAction.CallbackContext context) => ResumeActionPerformed?.Invoke();
	}
}