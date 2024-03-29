using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputStates
{
	[CreateAssetMenu(fileName = "GameplayInputState", menuName = "Input State/Gameplay Input State", order = 0)]
	public class GameplayInputState : InputState
	{
		private PlayerInputActions _playerInputActions;
		
		private InputAction.CallbackContext _latestMoveContext;
		private InputAction.CallbackContext _latestLookContext;
		private InputAction.CallbackContext _latestVerticalMoveContext;
        
		public event Action PauseActionPerformed;
		public event Action MoveActionStarted;
		public event Action MoveActionEnded;
		public event Action LookActionStarted;
		public event Action LookActionEnded;
		public event Action VerticalMoveActionStarted;
		public event Action VerticalMoveActionEnded;
		public event Action CameraLookActivatedStarted;
		public event Action CameraLookActivatedEnded;
		public event Action CameraZoomInPerformed;
		public event Action CameraZoomOutPerformed;

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
		
		public bool IsMoveActive { get; private set; }
		public bool IsLookActive { get; private set; }
		public bool IsVerticalMoveActive { get; private set; }
		public bool IsCameraLookActivated { get; private set; }
		
		public override void EnableInputs()
		{
			_playerInputActions ??= new PlayerInputActions();
			
			_playerInputActions.Gameplay.PauseGame.performed += OnPauseActionPerformed;
			
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

		public override void DisableInputs()
		{
			_playerInputActions.Gameplay.Disable();
			_playerInputActions.Gameplay.PauseGame.performed -= OnPauseActionPerformed;
			
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

		#region Pause Action
		
		private void OnPauseActionPerformed(InputAction.CallbackContext callbackContext) => PauseActionPerformed?.Invoke();

		#endregion

		#region Move Action

		private void OnMoveActionStarted(InputAction.CallbackContext callbackContext)
		{
			IsMoveActive = true;
			MoveActionStarted?.Invoke();
		}

		private void OnMoveActionCanceled(InputAction.CallbackContext callbackContext)
		{
			IsMoveActive = false;
			MoveActionEnded?.Invoke();
		}

		private void OnMoveActionPerformed(InputAction.CallbackContext callbackContext) => _latestMoveContext = callbackContext;

		#endregion

		#region Vertical Move Action

		private void OnVerticalMoveActionStarted(InputAction.CallbackContext callbackContext)
		{
			IsVerticalMoveActive = true;
			VerticalMoveActionStarted?.Invoke();
		}
		
		private void OnVerticalMoveActionCanceled(InputAction.CallbackContext callbackContext)
		{
			IsVerticalMoveActive = false;
			VerticalMoveActionEnded?.Invoke();
		}

		private void OnVerticalMoveActionPerformed(InputAction.CallbackContext callbackContext) => _latestVerticalMoveContext = callbackContext;

		#endregion

		#region Camera Look Action

		private void OnCameraLookActionStarted(InputAction.CallbackContext callbackContext)
		{
			IsLookActive = true;
			LookActionStarted?.Invoke();
		}

		private void OnCameraLookActionCanceled(InputAction.CallbackContext callbackContext)
		{
			IsLookActive = false;
			LookActionEnded?.Invoke();
		}

		private void OnCameraLookActionPerformed(InputAction.CallbackContext callbackContext) => _latestLookContext = callbackContext;

		#endregion

		#region Camera Look Activate Action

		private void OnCameraLookActivateActionStarted(InputAction.CallbackContext callbackContext)
		{
			IsCameraLookActivated = true;
			CameraLookActivatedStarted?.Invoke();
		}

		private void OnCameraLookActivateActionCanceled(InputAction.CallbackContext callbackContext)
		{
			IsCameraLookActivated = false;
			CameraLookActivatedEnded?.Invoke();
		}

		#endregion

		#region Camera Zoom Action

		private void OnCameraZoomActionPerformed(InputAction.CallbackContext callbackContext)
		{
			float inputValue = callbackContext.ReadValue<float>();
			if (inputValue < 0)
				CameraZoomInPerformed?.Invoke();
			else if (inputValue > 0)
				CameraZoomOutPerformed?.Invoke();
		}

		#endregion
	}
}