using System;
using InputStates.Enums;
using InputStates.InputEventArgs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputStates
{
	public class GameplayInputState : IInputState
	{
		private PlayerInputActions _playerInputActions = new();
		
		private InputAction.CallbackContext _latestMoveContext;
		private InputAction.CallbackContext _latestLookContext;
		private InputAction.CallbackContext _latestVerticalMoveContext;
        
		public event EventHandler MoveActionStarted;
		public event EventHandler MoveActionEnded;
		public event EventHandler LookActionStarted;
		public event EventHandler LookActionEnded;
		public event EventHandler VerticalMoveActionStarted;
		public event EventHandler VerticalMoveActionEnded;
		public event EventHandler CameraLookActivatedStarted;
		public event EventHandler CameraLookActivatedEnded;
		public event EventHandler<CameraZoomInputEventArgs> CameraZoomInputPerformed;

		public Vector3 CurrentMoveValue
		{
			get
			{
				Vector2 horizontalMoveValue = CurrentHorizontalMoveValue;
				return new Vector3(horizontalMoveValue.x, CurrentVerticalMoveValue, horizontalMoveValue.y);
			}
		}
		public Vector2 CurrentHorizontalMoveValue => _latestMoveContext.ReadValue<Vector2>();
		public Vector2 CurrentLookValue => _latestLookContext.ReadValue<Vector2>();
		public float CurrentVerticalMoveValue => _latestVerticalMoveContext.ReadValue<float>();

		public bool ShowInteractions => true;
		public bool IsMoveActive { get; private set; }
		public bool IsLookActive { get; private set; }
		public bool IsVerticalMoveActive { get; private set; }
		public bool IsCameraLookActivated { get; private set; }

		public void Enable()
		{
			_playerInputActions ??= new PlayerInputActions();
			
			_playerInputActions.Gameplay.Move.started += OnMoveActionStarted;
			_playerInputActions.Gameplay.Move.canceled += OnMoveActionCanceled;
			_playerInputActions.Gameplay.Move.performed += OnMoveActionPerformed;

			_playerInputActions.Gameplay.CameraLook.started += OnCameraLookActionStarted;
			_playerInputActions.Gameplay.CameraLook.canceled += OnCameraLookActionCanceled;
			_playerInputActions.Gameplay.CameraLook.performed += OnCameraLookActionPerformed;

			_playerInputActions.Gameplay.VerticalMove.started += OnVerticalMoveActionStarted;
			_playerInputActions.Gameplay.VerticalMove.canceled += OnVerticalMoveActionCanceled;
			_playerInputActions.Gameplay.VerticalMove.performed += OnVerticalMoveActionPerformed;

			_playerInputActions.Gameplay.CameraLookActivate.started += OnCameraLookActivateActionStarted;
			_playerInputActions.Gameplay.CameraLookActivate.canceled += OnCameraLookActivateActionCanceled;

			_playerInputActions.Gameplay.CameraZoom.performed += OnCameraZoomActionPerformed;
			
			_playerInputActions.Gameplay.Enable();
		}

		public void Disable()
		{
			_playerInputActions.Gameplay.Disable();
			
			_playerInputActions.Gameplay.Move.started -= OnMoveActionStarted;
			_playerInputActions.Gameplay.Move.canceled -= OnMoveActionCanceled;
			_playerInputActions.Gameplay.Move.performed -= OnMoveActionPerformed;
			
			_playerInputActions.Gameplay.CameraLook.started -= OnCameraLookActionStarted;
			_playerInputActions.Gameplay.CameraLook.canceled -= OnCameraLookActionCanceled;
			_playerInputActions.Gameplay.CameraLook.performed -= OnCameraLookActionPerformed;

			_playerInputActions.Gameplay.VerticalMove.started -= OnVerticalMoveActionStarted;
			_playerInputActions.Gameplay.VerticalMove.canceled -= OnVerticalMoveActionCanceled;
			_playerInputActions.Gameplay.VerticalMove.performed -= OnVerticalMoveActionPerformed;

			_playerInputActions.Gameplay.CameraLookActivate.started -= OnCameraLookActivateActionStarted;
			_playerInputActions.Gameplay.CameraLookActivate.canceled -= OnCameraLookActivateActionCanceled;

			_playerInputActions.Gameplay.CameraZoom.performed -= OnCameraZoomActionPerformed;
		}

		private void OnMoveActionStarted(InputAction.CallbackContext callbackContext)
		{
			IsMoveActive = true;
			MoveActionStarted?.Invoke(this, EventArgs.Empty);
		}

		private void OnMoveActionCanceled(InputAction.CallbackContext callbackContext)
		{
			IsMoveActive = false;
			MoveActionEnded?.Invoke(this, EventArgs.Empty);
		}

		private void OnMoveActionPerformed(InputAction.CallbackContext callbackContext) => _latestMoveContext = callbackContext;

		private void OnVerticalMoveActionStarted(InputAction.CallbackContext callbackContext)
		{
			IsVerticalMoveActive = true;
			VerticalMoveActionStarted?.Invoke(this, EventArgs.Empty);
		}
		
		private void OnVerticalMoveActionCanceled(InputAction.CallbackContext callbackContext)
		{
			IsVerticalMoveActive = false;
			VerticalMoveActionEnded?.Invoke(this, EventArgs.Empty);
		}

		private void OnVerticalMoveActionPerformed(InputAction.CallbackContext callbackContext) => _latestVerticalMoveContext = callbackContext;

		private void OnCameraLookActionStarted(InputAction.CallbackContext callbackContext)
		{
			IsLookActive = true;
			LookActionStarted?.Invoke(this, EventArgs.Empty);
		}

		private void OnCameraLookActionCanceled(InputAction.CallbackContext callbackContext)
		{
			IsLookActive = false;
			LookActionEnded?.Invoke(this, EventArgs.Empty);
		}

		private void OnCameraLookActionPerformed(InputAction.CallbackContext callbackContext) => _latestLookContext = callbackContext;

		private void OnCameraLookActivateActionStarted(InputAction.CallbackContext callbackContext)
		{
			IsCameraLookActivated = true;
			CameraLookActivatedStarted?.Invoke(this, EventArgs.Empty);
		}

		private void OnCameraLookActivateActionCanceled(InputAction.CallbackContext callbackContext)
		{
			IsCameraLookActivated = false;
			CameraLookActivatedEnded?.Invoke(this, EventArgs.Empty);
		}

		private void OnCameraZoomActionPerformed(InputAction.CallbackContext callbackContext)
		{
			float inputValue = callbackContext.ReadValue<float>();
			if (inputValue == 0)
				return;

			ZoomType zoomType = inputValue < 0 ? ZoomType.In : ZoomType.Out;
			CameraZoomInputPerformed?.Invoke(this, new CameraZoomInputEventArgs(zoomType));
		}
	}
}