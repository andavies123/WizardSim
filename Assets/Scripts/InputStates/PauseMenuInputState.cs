using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputStates
{
	[CreateAssetMenu(fileName = "PauseMenuInputState", menuName = "Input State/Pause Menu Input State", order = 0)]
	public class PauseMenuInputState : InputState
	{
		private PlayerInputActions _playerInputActions;
		
		public event Action ResumeActionPerformed;

		public override void EnableInputs()
		{
			_playerInputActions ??= new PlayerInputActions();

			_playerInputActions.PauseMenu.ResumeGame.performed += OnResumeActionPerformed;
			
			_playerInputActions.PauseMenu.Enable();
		}

		public override void DisableInputs()
		{
			_playerInputActions.PauseMenu.Disable();

			_playerInputActions.PauseMenu.ResumeGame.performed -= OnResumeActionPerformed;
		}

		#region Resume Action

			private void OnResumeActionPerformed(InputAction.CallbackContext context) => ResumeActionPerformed?.Invoke();
		
		#endregion
	}
}