using System;
using UnityEngine.InputSystem;

namespace InputStates
{
	public class InteractionInput : IInput
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private PlayerInputActions.InteractionActions interaction;
		
		public event EventHandler CancelInteractionActionPerformed;

		public InteractionInput() => interaction = _playerInputActions.Interaction;
		
		public bool ShowInteractions => true;

		public void Enable()
		{
			interaction.CancelInteraction.performed += OnCancelInteractionActionPerformed;
			
			interaction.Enable();
		}

		public void Disable()
		{
			interaction.Disable();

			interaction.CancelInteraction.performed -= OnCancelInteractionActionPerformed;
		}

		private void OnCancelInteractionActionPerformed(InputAction.CallbackContext context) => 
			CancelInteractionActionPerformed?.Invoke(this, EventArgs.Empty);
	}
}