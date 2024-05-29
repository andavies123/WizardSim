﻿using System;
using UnityEngine.InputSystem;

namespace InputStates
{
	public class TaskManagementInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private PlayerInputActions.TaskManagementActions _taskManagement;

		public TaskManagementInputState()
		{
			_taskManagement = _playerInputActions.TaskManagement;
		}

		public event EventHandler CloseWindowRequested;
        
		public bool ShowInteractions => false;
		
		public void Enable()
		{
			_taskManagement.CloseWindow.performed += OnCloseWindowActionPerformed;
			
			_taskManagement.Enable();
		}

		public void Disable()
		{
			_taskManagement.Disable();
			
			_taskManagement.CloseWindow.performed -= OnCloseWindowActionPerformed;
		}

		private void OnCloseWindowActionPerformed(InputAction.CallbackContext context)
		{
			CloseWindowRequested?.Invoke(this, EventArgs.Empty);
		}
	}
}