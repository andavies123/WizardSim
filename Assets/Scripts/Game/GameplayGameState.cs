using System;
using CameraComponents;
using InputStates;
using UI;
using UIManagers;
using UnityEngine;

namespace Game
{
	public class GameplayGameState : IGameState
	{
		private readonly InputStateMachine _inputStateMachine = new();
		
		private readonly GameplayUIState _gameplayUIState;
		private readonly GameplayInput _gameplayInput;
		private readonly InteractionInput _interactionInput;

		private readonly InteractableRaycaster _interactableRaycaster;
		private readonly InteractionEvents _interactionEvents;

		private Action<MonoBehaviour> _interactionCallback;
		
		public event EventHandler PauseGameRequested;
		
		public GameplayGameState(
			GameplayUIState gameplayUIState,
			InteractableRaycaster interactableRaycaster,
			InteractionEvents interactionEvents)
		{
			_gameplayUIState = gameplayUIState;
			_gameplayInput = Dependencies.GetDependency<GameplayInput>();
			_interactionInput = Dependencies.GetDependency<InteractionInput>();
			_interactableRaycaster = interactableRaycaster;
			_interactionEvents = interactionEvents;
		}

		public void Enable()
		{
			_gameplayUIState.PauseButtonPressed += OnUIPauseButtonPressed;
			_gameplayUIState.Enable();
			
			_gameplayInput.PauseActionPerformed += OnGameplayInputPauseActionPerformed;
			_interactionInput.CancelInteractionActionPerformed += OnInteractionInputCancelActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary += OnInteractablePrimaryActionSelected;
			_interactableRaycaster.NonInteractableSelectedPrimary += OnNonInteractablePrimaryActionSelected;

			_interactionEvents.InteractionRequested += OnInteractionRequested;
			_interactionEvents.InteractionEnded += OnInteractionEnded;
			
			_inputStateMachine.SetCurrentState(_gameplayInput);
		}

		public void Disable()
		{
			_gameplayUIState.Disable();
			_gameplayUIState.PauseButtonPressed -= OnUIPauseButtonPressed;

			_gameplayInput.PauseActionPerformed -= OnGameplayInputPauseActionPerformed;
			_interactionInput.CancelInteractionActionPerformed -= OnInteractionInputCancelActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary -= OnInteractablePrimaryActionSelected;
			_interactableRaycaster.NonInteractableSelectedPrimary -= OnNonInteractablePrimaryActionSelected;

			_interactionEvents.InteractionRequested -= OnInteractionRequested;
			_interactionEvents.InteractionEnded -= OnInteractionEnded;
			
			_inputStateMachine.SetCurrentState(null);
		}

		private void StartInteraction(Action<MonoBehaviour> interactionCallback)
		{
			_interactionCallback = interactionCallback;
			_inputStateMachine.SetCurrentState(_interactionInput);
		}

		private void EndInteraction()
		{
			_interactionCallback = null;
			_inputStateMachine.SetCurrentState(_gameplayInput);
		}

		private void OnUIPauseButtonPressed(object sender, EventArgs args) => RaisePauseGameRequested(sender);
		private void OnGameplayInputPauseActionPerformed(object sender, EventArgs args) => RaisePauseGameRequested(sender);
		private void OnInteractionInputCancelActionPerformed(object sender, EventArgs args) => EndInteraction();

		private void OnInteractablePrimaryActionSelected(object sender, InteractableRaycasterEventArgs args)
		{
			if (_interactionCallback != null)
			{
				_interactionCallback.Invoke(args.Interactable);
				args.Interactable.IsSelected = false; // Gets marked as selected
			}
			else
			{
				_gameplayUIState.CloseContextMenu();
				_gameplayUIState.OpenInfoWindow(args.Interactable);
			}
		}

		private void OnNonInteractablePrimaryActionSelected(object sender, EventArgs args)
		{
			if (!_gameplayUIState.IsMouseOverGameObject)
			{
				_gameplayUIState.CloseInfoWindow();
				_gameplayUIState.CloseContextMenu();	
			}
		}

		private void OnInteractionRequested(object sender, InteractionRequestEventArgs args) => StartInteraction(args.OnInteractionCallback);
		private void OnInteractionEnded(object sender, EventArgs args) => EndInteraction();
        
		private void RaisePauseGameRequested(object sender) => PauseGameRequested?.Invoke(sender, EventArgs.Empty);
	}
}