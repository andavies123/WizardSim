using System;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputStates
{
	[CreateAssetMenu(menuName = "Input State/Interaction Input State", fileName = "InteractionInputState", order = 0)]
	public class InteractionInputState : InputState
	{
		private PlayerInputActions _playerInputActions;
		
		public event Action CancelInteractionActionPerformed;

		public event Action<MonoBehaviour> InteractableSelected;

		public override void EnableInputs()
		{
			_playerInputActions ??= new PlayerInputActions();

			_playerInputActions.Interaction.CancelInteraction.performed += OnCancelInteractionActionPerformed;
			
			_playerInputActions.Interaction.Enable();

			Interactable.LeftMousePressed += OnInteractableLeftMousePressed;
		}

		public override void DisableInputs()
		{
			_playerInputActions.Interaction.Disable();

			_playerInputActions.Interaction.CancelInteraction.performed -= OnCancelInteractionActionPerformed;

			Interactable.LeftMousePressed -= OnInteractableLeftMousePressed;
		}

		#region Cancel Interaction Action

		private void OnCancelInteractionActionPerformed(InputAction.CallbackContext context) => CancelInteractionActionPerformed?.Invoke();
		
		#endregion

		private void OnInteractableLeftMousePressed(MonoBehaviour monoBehaviour) => InteractableSelected?.Invoke(monoBehaviour);
	}
}