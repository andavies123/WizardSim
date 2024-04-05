using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace UI
{
	/// <summary>
	/// Helpful class that can be put on the same GameObject as the collider and expose the
	/// Unity Mouse Events
	/// </summary>
	public class MouseInteractionEvents : MonoBehaviour
	{
		public event Action MouseEntered;
		public event Action MouseExited;
		public event Action LeftMousePressed;
		public event Action LeftMouseReleased;
		public event Action RightMousePressed;
		public event Action RightMouseReleased;

		private void OnMouseEnter() => MouseEntered?.Invoke();
		private void OnMouseExit() => MouseExited?.Invoke();

		private void OnMouseOver()
		{
			if (Input.GetMouseButtonDown((int) MouseButton.Left))
				LeftMousePressed?.Invoke();
			else if (Input.GetMouseButtonUp((int) MouseButton.Left))
				LeftMouseReleased?.Invoke();
			
			if (Input.GetMouseButtonDown((int) MouseButton.Right))
				RightMousePressed?.Invoke();
			else if (Input.GetMouseButtonUp((int) MouseButton.Right))
				RightMouseReleased?.Invoke();
		}
	}
}