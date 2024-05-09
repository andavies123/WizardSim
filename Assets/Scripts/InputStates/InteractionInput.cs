using System;
using CameraComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputStates
{
	public class InteractionInput : IInput
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private readonly InteractableRaycaster _interactableRaycaster;
		
		private PlayerInputActions.InteractionActions interaction;
		
		public event EventHandler CancelInteractionActionPerformed;
		
		public InteractionInput(InteractableRaycaster interactableRaycaster)
		{
			_interactableRaycaster = interactableRaycaster;
			interaction = _playerInputActions.Interaction;
		}

		public Action<MonoBehaviour> InteractionCallback { get; set; }
		public bool ShowInteractions => true;

		public void Enable()
		{
			interaction.CancelInteraction.performed += OnCancelInteractionActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary += OnInteractableSelectedPrimary;
			
			interaction.Enable();
		}

		public void Disable()
		{
			interaction.Disable();

			interaction.CancelInteraction.performed -= OnCancelInteractionActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary -= OnInteractableSelectedPrimary;
		}

		private void OnCancelInteractionActionPerformed(InputAction.CallbackContext context) => 
			CancelInteractionActionPerformed?.Invoke(this, EventArgs.Empty);

		private void OnInteractableSelectedPrimary(object sender, InteractableRaycasterEventArgs args)
		{
			if (InteractionCallback != null)
			{
				InteractionCallback.Invoke(args.Interactable);
				args.Interactable.IsSelected = false; // Gets marked as selected
			}
		}
	}
}