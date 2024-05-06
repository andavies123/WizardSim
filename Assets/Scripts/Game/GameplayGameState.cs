using System;
using CameraComponents;
using Game.Messages;
using Game.MessengerSystem;
using GameWorld.EventArgs;
using GameWorld.Messages;
using InputStates;
using UI;
using UIStates;
using UnityEngine;

namespace Game
{
	public class GameplayGameState : IGameState
	{
		private readonly InputStateMachine _inputStateMachine = new();
		
		private readonly GameplayUIState _gameplayUIState;
		
		private readonly GameplayInput _gameplayInput;
		private readonly InteractionInput _interactionInput;
		private readonly PlacementModeInput _placementModeInput;

		private readonly InteractableRaycaster _interactableRaycaster;
		private readonly InteractionEvents _interactionEvents;

		private Action<MonoBehaviour> _interactionCallback;
		private GameObject _placementPrefab;
		
		public event EventHandler PauseGameRequested;
		
		public GameplayGameState(
			GameplayUIState gameplayUIState,
			InteractableRaycaster interactableRaycaster,
			InteractionEvents interactionEvents)
		{
			_gameplayUIState = gameplayUIState;
			_interactableRaycaster = interactableRaycaster;
			_interactionEvents = interactionEvents;
			
			_gameplayInput = new GameplayInput();
			_interactionInput = new InteractionInput();
			_placementModeInput = new PlacementModeInput(_interactableRaycaster);
			
			Dependencies.RegisterDependency(_gameplayInput);
			Dependencies.RegisterDependency(_interactionInput);
			Dependencies.RegisterDependency(_placementModeInput);
		}

		public void Enable()
		{
			_gameplayUIState.PauseButtonPressed += OnUIPauseButtonPressed;
			_gameplayUIState.Enable();
			
			_gameplayInput.PauseActionPerformed += OnGameplayInputPauseActionPerformed;
			_interactionInput.CancelInteractionActionPerformed += OnInteractionInputCancelActionPerformed;
			
			_placementModeInput.EndPlacementModeActionPerformed += OnPlacementModeInputEndActionPerformed;
			_placementModeInput.PreviewPositionUpdated += OnPlacementModeInputPreviewUpdated;
			_placementModeInput.PlacementRequested += OnPlacementModeInputPlacementRequested;
			_placementModeInput.HidePlacementPreviewRequested += OnHidePlacementPreviewRequested;

			_interactableRaycaster.InteractableSelectedPrimary += OnInteractablePrimaryActionSelected;
			_interactableRaycaster.NonInteractableSelectedPrimary += OnNonInteractablePrimaryActionSelected;

			_interactionEvents.InteractionRequested += OnInteractionRequested;
			_interactionEvents.InteractionEnded += OnInteractionEnded;
			
			GlobalMessenger.Subscribe<BeginPlacementModeRequest>(OnBeginPlacementModeRequestReceived);
			GlobalMessenger.Subscribe<EndPlacementModeRequest>(OnEndPlacementModeRequestReceived);
			
			_inputStateMachine.SetCurrentState(_gameplayInput);
		}

		public void Disable()
		{
			_gameplayUIState.Disable();
			_gameplayUIState.PauseButtonPressed -= OnUIPauseButtonPressed;

			_gameplayInput.PauseActionPerformed -= OnGameplayInputPauseActionPerformed;
			_interactionInput.CancelInteractionActionPerformed -= OnInteractionInputCancelActionPerformed;
			
			_placementModeInput.EndPlacementModeActionPerformed -= OnPlacementModeInputEndActionPerformed;
			_placementModeInput.PreviewPositionUpdated -= OnPlacementModeInputPreviewUpdated;
			_placementModeInput.PlacementRequested -= OnPlacementModeInputPlacementRequested;
			_placementModeInput.HidePlacementPreviewRequested -= OnHidePlacementPreviewRequested;

			_interactableRaycaster.InteractableSelectedPrimary -= OnInteractablePrimaryActionSelected;
			_interactableRaycaster.NonInteractableSelectedPrimary -= OnNonInteractablePrimaryActionSelected;

			_interactionEvents.InteractionRequested -= OnInteractionRequested;
			_interactionEvents.InteractionEnded -= OnInteractionEnded;
			
			GlobalMessenger.Unsubscribe<BeginPlacementModeRequest>(OnBeginPlacementModeRequestReceived);
			GlobalMessenger.Unsubscribe<EndPlacementModeRequest>(OnEndPlacementModeRequestReceived);
			
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

		private void StartPlacementMode(GameObject placementPrefab)
		{
			_placementPrefab = placementPrefab;
			_inputStateMachine.SetCurrentState(_placementModeInput);
		}

		private void EndPlacementMode()
		{
			_inputStateMachine.SetCurrentState(_gameplayInput);
			_placementPrefab = null;
			GlobalMessenger.Publish(new PlacementModeEnded());
		}

		private void OnBeginPlacementModeRequestReceived(BeginPlacementModeRequest request) => StartPlacementMode(request.PlacementPrefab);
		private void OnEndPlacementModeRequestReceived(EndPlacementModeRequest request) => EndPlacementMode();

		private void OnUIPauseButtonPressed(object sender, EventArgs args) => RaisePauseGameRequested(sender);
		private void OnGameplayInputPauseActionPerformed(object sender, EventArgs args) => RaisePauseGameRequested(sender);
		private void OnPlacementModeInputEndActionPerformed(object sender, EventArgs args) => EndPlacementMode();
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

		private void OnPlacementModeInputPreviewUpdated(object sender, WorldPositionEventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectPreviewRequest(args.ChunkPosition, args.TilePosition, _placementPrefab));

		private void OnPlacementModeInputPlacementRequested(object sender, WorldPositionEventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectPlacementRequest(args.ChunkPosition, args.TilePosition, _placementPrefab));

		private static void OnHidePlacementPreviewRequested(object sender, EventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectHidePreviewRequest());

		private void OnInteractionRequested(object sender, InteractionRequestEventArgs args) => StartInteraction(args.OnInteractionCallback);
		private void OnInteractionEnded(object sender, EventArgs args) => EndInteraction();
        
		private void RaisePauseGameRequested(object sender) => PauseGameRequested?.Invoke(sender, EventArgs.Empty);
	}
}