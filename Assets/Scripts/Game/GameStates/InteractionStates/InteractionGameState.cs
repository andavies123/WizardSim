using System;
using CameraComponents;
using Extensions;
using UnityEngine;

namespace Game.GameStates.InteractionStates
{
	public class InteractionGameState : GameState
	{
		private readonly InteractionUIState _interactionUIState;
		private readonly InteractionInputState _interactionInputState;
		
		public event EventHandler InteractionCanceled
		{
			add => _interactionInputState.CancelInteractionActionPerformed += value;
			remove => _interactionInputState.CancelInteractionActionPerformed -= value;
		}
		
		public InteractionGameState(InteractionUIState uiState, InteractableRaycaster interactableRaycaster)
		{
			interactableRaycaster.ThrowIfNull(nameof(interactableRaycaster));

			_interactionUIState = uiState.ThrowIfNull(nameof(uiState));
			_interactionInputState = new InteractionInputState(interactableRaycaster);
		}

		public override bool AllowCameraInputs => true;
		public override bool AllowInteractions => true;

		protected override UIState UIState => _interactionUIState;
		protected override IInputState InputState => _interactionInputState;

		public void SetInteractionCallback(Action<MonoBehaviour> interactionCallback) => 
			_interactionInputState.InteractionCallback = interactionCallback;
        
		protected override void OnEnabled() { }
		protected override void OnDisabled() { }
	}
}