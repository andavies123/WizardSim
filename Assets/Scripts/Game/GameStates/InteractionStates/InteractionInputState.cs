﻿using System;
using CameraComponents;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.InputSystem.PlayerInputActions;

namespace Game.GameStates.InteractionStates
{
	public class InteractionInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private readonly InteractableRaycaster _interactableRaycaster;
		
		private InteractionActions _interaction;
		
		public event EventHandler CancelInteractionActionPerformed;
		
		public InteractionInputState(InteractableRaycaster interactableRaycaster)
		{
			_interactableRaycaster = interactableRaycaster;
			_interaction = _playerInputActions.Interaction;
		}

		public Action<MonoBehaviour> InteractionCallback { get; set; }
		public bool ShowInteractions => true;

		public void Enable()
		{
			_interaction.CancelInteraction.performed += OnCancelInteractionActionPerformed;
			_interactableRaycaster.InteractableSelectedPrimary += OnInteractableSelectedPrimary;
			
			_interaction.Enable();
		}

		public void Disable()
		{
			_interaction.Disable();

			_interaction.CancelInteraction.performed -= OnCancelInteractionActionPerformed;
			_interactableRaycaster.InteractableSelectedPrimary -= OnInteractableSelectedPrimary;
		}

		private void OnCancelInteractionActionPerformed(CallbackContext context)
		{
			CancelInteractionActionPerformed?.Invoke(this, EventArgs.Empty);
		}

		private void OnInteractableSelectedPrimary(object sender, InteractableRaycasterEventArgs args)
		{
			if (InteractionCallback != null)
			{
				InteractionCallback.Invoke(args.Interactable);
				args.Interactable.IsSelected = false; // Gets marked as not selected
			}
		}
	}
}