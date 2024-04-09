using System;
using UnityEngine.InputSystem;

namespace InputStates
{
	public class InteractionInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		
		public event Action CancelInteractionActionPerformed;
		
		public bool ShowInteractions => true;

		public void Enable()
		{
			_playerInputActions.Interaction.CancelInteraction.performed += OnCancelInteractionActionPerformed;
			
			_playerInputActions.Interaction.Enable();
		}

		public void Disable()
		{
			_playerInputActions.Interaction.Disable();

			_playerInputActions.Interaction.CancelInteraction.performed -= OnCancelInteractionActionPerformed;
		}

		private void OnCancelInteractionActionPerformed(InputAction.CallbackContext context) => CancelInteractionActionPerformed?.Invoke();
	}
}