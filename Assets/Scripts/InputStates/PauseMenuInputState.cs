using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputStates
{
	[CreateAssetMenu(fileName = "PauseMenuInputState", menuName = "Input State/Pause Menu Input State", order = 0)]
	public class PauseMenuInputState : InputState
	{
		public event Action ResumeActionPerformed;
		
		public void OnResumeAction(InputAction.CallbackContext context)
		{
			if (context.performed)
				ResumeActionPerformed?.Invoke();
		}
	}
}