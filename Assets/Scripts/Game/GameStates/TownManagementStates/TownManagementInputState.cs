using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.InputSystem.PlayerInputActions;

namespace Game.GameStates.TownManagementStates
{
	public class TownManagementInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private TownManagementActions _townManagement;

		public TownManagementInputState()
		{
			_townManagement = _playerInputActions.TownManagement;
		}

		public event EventHandler CloseWindowRequested;
        
		public bool ShowInteractions => false;
		
		public void Enable()
		{
			_townManagement.CloseWindow.performed += OnCloseWindowActionPerformed;
			
			_townManagement.Enable();
		}

		public void Disable()
		{
			_townManagement.Disable();
			
			_townManagement.CloseWindow.performed -= OnCloseWindowActionPerformed;
		}

		private void OnCloseWindowActionPerformed(CallbackContext context) =>
			CloseWindowRequested?.Invoke(this, EventArgs.Empty);
	}
}