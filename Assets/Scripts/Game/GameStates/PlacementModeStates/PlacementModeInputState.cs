﻿using System;
using CameraComponents;
using GameWorld.Builders.Chunks;
using GameWorld.GameWorldEventArgs;
using GameWorld.Tiles;
using UnityEngine.InputSystem;

namespace Game.GameStates.PlacementModeStates
{
	public class PlacementModeInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private readonly InteractableRaycaster _interactableRaycaster;
		private PlayerInputActions.PlacementModeActions _placementMode;
		
		public PlacementModeInputState(InteractableRaycaster interactableRaycaster)
		{
			_interactableRaycaster = interactableRaycaster;
			_placementMode = _playerInputActions.PlacementMode;
		}
		
		public event EventHandler<WorldPositionEventArgs> PlacementRequested;
		public event EventHandler EndPlacementModeActionPerformed;
		
		public event EventHandler<WorldPositionEventArgs> PreviewPositionUpdated;
		public event EventHandler<bool> PreviewVisibilityUpdated;
        
		public bool ShowInteractions => true;

		public void Enable()
		{
			_interactableRaycaster.InteractableHoverBegin += OnInteractableHoverBegin;
			_interactableRaycaster.UIHoverBegin += OnUIHoverBegin;
			_interactableRaycaster.InteractableSelectedPrimary += OnInteractableSelectedPrimary;
			_interactableRaycaster.NonInteractableHoverBegin += OnNonInteractableHoverBegin;
			
			_placementMode.EndPlacementMode.performed += OnEndPlacementModeActionPerformed;
			
			_placementMode.Enable();
		}

		public void Disable()
		{
			_placementMode.Disable();

			_interactableRaycaster.InteractableHoverBegin -= OnInteractableHoverBegin;
			_interactableRaycaster.UIHoverBegin -= OnUIHoverBegin;
			_interactableRaycaster.InteractableSelectedPrimary -= OnInteractableSelectedPrimary;
			_interactableRaycaster.NonInteractableHoverBegin -= OnNonInteractableHoverBegin;
			
			_placementMode.EndPlacementMode.performed -= OnEndPlacementModeActionPerformed;
		}

		private void OnEndPlacementModeActionPerformed(InputAction.CallbackContext context) =>
			EndPlacementModeActionPerformed?.Invoke(this, EventArgs.Empty);

		private void OnInteractableHoverBegin(object sender, InteractableRaycasterEventArgs args)
		{
			if (args.Interactable.TryGetComponent(out Chunk chunk))
			{
				PreviewPositionUpdated?.Invoke(this, new WorldPositionEventArgs(chunk));
				PreviewVisibilityUpdated?.Invoke(this, true);
			}
			else
			{
				PreviewVisibilityUpdated?.Invoke(this, false);
			}
		}

		private void OnUIHoverBegin(object sender, EventArgs args)
		{
			PreviewVisibilityUpdated?.Invoke(this, false);
		}

		private void OnInteractableSelectedPrimary(object sender, InteractableRaycasterEventArgs args)
		{
			if (args.Interactable.TryGetComponent(out Tile tile))
			{
				PlacementRequested?.Invoke(this, new WorldPositionEventArgs(tile));
			}
		}

		private void OnNonInteractableHoverBegin(object sender, EventArgs args) =>
			PreviewVisibilityUpdated?.Invoke(this, false);
	}
}