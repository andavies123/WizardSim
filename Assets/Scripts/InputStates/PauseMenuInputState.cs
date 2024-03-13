using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputStates
{
	[CreateAssetMenu(fileName = "PauseMenuInputState", menuName = "Input State/Pause Menu Input State", order = 0)]
	public class PauseMenuInputState : InputState
	{
		public event Action ResumeKeyPressed;
		
		public void OnResumeKeyPressed(InputAction.CallbackContext context)
		{
			if (context.performed)
				ResumeKeyPressed?.Invoke();
		}
	}
}