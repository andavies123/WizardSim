using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputStates
{
	[CreateAssetMenu(fileName = "GameplayInputState", menuName = "Input State/Gameplay Input State", order = 0)]
	public class GameplayInputState : InputState
	{
		public event Action PauseKeyPressed;
		
		public void OnPauseKeyPressed(InputAction.CallbackContext context)
		{
			if (context.performed)
				PauseKeyPressed?.Invoke();
		}
	}
}