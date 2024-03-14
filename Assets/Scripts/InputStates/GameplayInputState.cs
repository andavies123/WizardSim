using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputStates
{
	[CreateAssetMenu(fileName = "GameplayInputState", menuName = "Input State/Gameplay Input State", order = 0)]
	public class GameplayInputState : InputState
	{
		private InputAction.CallbackContext _latestMoveContext;
		private InputAction.CallbackContext _latestLookContext;
		private InputAction.CallbackContext _latestVerticalMoveContext;
        
		public event Action PauseActionPerformed;
		public event Action MoveActionStarted;
		public event Action MoveActionEnded;
		public event Action LookActionStarted;
		public event Action LookActionEnded;
		public event Action VerticalMoveActionStarted;
		public event Action VerticalMoveActionStopped;
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
		
		public void OnPauseAction(InputAction.CallbackContext context)
		{
			if (context.performed)
				PauseActionPerformed?.Invoke();
		}

		public void OnMoveAction(InputAction.CallbackContext context)
		{
			_latestMoveContext = context;

			if (context.started)
			{
				IsMoveActive = true;
				MoveActionStarted?.Invoke();
			}
			else if (context.canceled)
			{
				IsMoveActive = false;
				MoveActionEnded?.Invoke();
			}
		}

		public void OnLookAction(InputAction.CallbackContext context)
		{
			_latestLookContext = context;

			if (context.started)
			{
				IsLookActive = true;
				LookActionStarted?.Invoke();
			}
			else if (context.canceled)
			{
				IsLookActive = false;
				LookActionEnded?.Invoke();
			}
		}

		public void OnVerticalMoveAction(InputAction.CallbackContext context)
		{
			_latestVerticalMoveContext = context;

			if (context.started)
			{
				IsVerticalMoveActive = true;
				VerticalMoveActionStarted?.Invoke();
			}
			else if (context.canceled)
			{
				IsVerticalMoveActive = false;
				VerticalMoveActionStopped?.Invoke();
			}
		}

		public void OnCameraLookActivateAction(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				IsCameraLookActivated = true;
				CameraLookActivatedStarted?.Invoke();
			}
			else if (context.canceled)
			{
				IsCameraLookActivated = false;
				CameraLookActivatedEnded?.Invoke();
			}
		}

		public void OnCameraZoomAction(InputAction.CallbackContext context)
		{
			if (!context.performed)
				return;
			
			float inputValue = context.ReadValue<float>();
			if (inputValue < 0)
				CameraZoomInPerformed?.Invoke();
			else if (inputValue > 0)
				CameraZoomOutPerformed?.Invoke();
		}
	}
}