using System;
using CameraComponents;
using Game.Messages;
using Game.MessengerSystem;
using GameWorld.EventArgs;
using GameWorld.Messages;
using InputStates;
using InputStates.InputEventArgs;
using UI.Messages;
using UIStates;
using UnityEngine;

namespace Game
{
	public class GameplayGameState : IGameState
	{
		private readonly InputStateMachine _mainInputStateMachine = new();
		private readonly InputStateMachine _subInputStateMachine = new();
		
		private readonly GameplayUIState _gameplayUIState;
		
		private readonly GameplayInput _gameplayInput;
		private readonly SecondaryGameplayInput _secondaryGameplayInput;
		private readonly InteractionInput _interactionInput;
		private readonly PlacementModeInput _placementModeInput;

		private GameObject _placementPrefab;
		
		public event EventHandler PauseGameRequested;
		
		public GameplayGameState(GameplayUIState gameplayUIState, InteractableRaycaster interactableRaycaster)
		{
			_gameplayUIState = gameplayUIState;
			
			_gameplayInput = new GameplayInput();
			_secondaryGameplayInput = new SecondaryGameplayInput(interactableRaycaster, _gameplayUIState);
			_interactionInput = new InteractionInput(interactableRaycaster);
			_placementModeInput = new PlacementModeInput(interactableRaycaster);
			
			Dependencies.RegisterDependency(_gameplayInput);
			Dependencies.RegisterDependency(_secondaryGameplayInput);
			Dependencies.RegisterDependency(_interactionInput);
			Dependencies.RegisterDependency(_placementModeInput);
		}

		public void Enable()
		{
			_gameplayUIState.PauseButtonPressed += OnUIPauseButtonPressed;
			_gameplayUIState.Enable();
			
			_secondaryGameplayInput.PauseActionPerformed += OnGameplayInputPauseActionPerformed;
			_secondaryGameplayInput.OpenInfoWindowRequested += OnGameplayOpenInfoWindowRequested;
			_secondaryGameplayInput.OpenContextMenuRequested += OnGameplayOpenContextMenuRequested;
			_secondaryGameplayInput.CloseInfoWindowRequested += OnGameplayCloseInfoWindowRequested;
			_secondaryGameplayInput.CloseContextMenuRequested += OnGameplayCloseContextMenuRequested;
			
			_interactionInput.CancelInteractionActionPerformed += OnInteractionInputCancelActionPerformed;
			
			_placementModeInput.EndPlacementModeActionPerformed += OnPlacementModeInputEndActionPerformed;
			_placementModeInput.PreviewPositionUpdated += OnPlacementModeInputPreviewUpdated;
			_placementModeInput.PlacementRequested += OnPlacementModeInputPlacementRequested;
			_placementModeInput.HidePlacementPreviewRequested += OnHidePlacementPreviewRequested;
			
			GlobalMessenger.Subscribe<BeginPlacementModeRequest>(OnBeginPlacementModeRequestReceived);
			GlobalMessenger.Subscribe<EndPlacementModeRequest>(OnEndPlacementModeRequestReceived);
			GlobalMessenger.Subscribe<StartInteractionRequest>(OnStartInteractionRequestReceived);
			GlobalMessenger.Subscribe<EndInteractionRequest>(OnEndInteractionRequestReceived);
			
			_mainInputStateMachine.SetCurrentState(_gameplayInput);
			_subInputStateMachine.SetCurrentState(_secondaryGameplayInput);
		}

		public void Disable()
		{
			_gameplayUIState.Disable();
			_gameplayUIState.PauseButtonPressed -= OnUIPauseButtonPressed;

			_secondaryGameplayInput.PauseActionPerformed -= OnGameplayInputPauseActionPerformed;
			_secondaryGameplayInput.OpenInfoWindowRequested -= OnGameplayOpenInfoWindowRequested;
			_secondaryGameplayInput.OpenContextMenuRequested -= OnGameplayOpenContextMenuRequested;
			_secondaryGameplayInput.CloseInfoWindowRequested -= OnGameplayCloseInfoWindowRequested;
			_secondaryGameplayInput.CloseContextMenuRequested -= OnGameplayCloseContextMenuRequested;
			
			_interactionInput.CancelInteractionActionPerformed -= OnInteractionInputCancelActionPerformed;
			
			_placementModeInput.EndPlacementModeActionPerformed -= OnPlacementModeInputEndActionPerformed;
			_placementModeInput.PreviewPositionUpdated -= OnPlacementModeInputPreviewUpdated;
			_placementModeInput.PlacementRequested -= OnPlacementModeInputPlacementRequested;
			_placementModeInput.HidePlacementPreviewRequested -= OnHidePlacementPreviewRequested;
			
			GlobalMessenger.Unsubscribe<BeginPlacementModeRequest>(OnBeginPlacementModeRequestReceived);
			GlobalMessenger.Unsubscribe<EndPlacementModeRequest>(OnEndPlacementModeRequestReceived);
			GlobalMessenger.Unsubscribe<StartInteractionRequest>(OnStartInteractionRequestReceived);
			GlobalMessenger.Unsubscribe<EndInteractionRequest>(OnEndInteractionRequestReceived);
			
			_mainInputStateMachine.SetCurrentState(null);
		}

		private void StartInteraction(Action<MonoBehaviour> interactionCallback)
		{
			_interactionInput.InteractionCallback = interactionCallback;
			_subInputStateMachine.SetCurrentState(_interactionInput);
		}

		private void EndInteraction()
		{
			_interactionInput.InteractionCallback = null;
			_subInputStateMachine.SetCurrentState(_secondaryGameplayInput);
		}

		private void StartPlacementMode(GameObject placementPrefab)
		{
			_placementPrefab = placementPrefab;
			_subInputStateMachine.SetCurrentState(_placementModeInput);
		}

		private void EndPlacementMode()
		{
			_subInputStateMachine.SetCurrentState(_secondaryGameplayInput);
			_placementPrefab = null;
			GlobalMessenger.Publish(new PlacementModeEnded());
		}
		
		private void RaisePauseGameRequested(object sender) => PauseGameRequested?.Invoke(sender, EventArgs.Empty);
		private void OnUIPauseButtonPressed(object sender, EventArgs args) => RaisePauseGameRequested(sender);

		#region Secondary Gameplay Input Callbacks

		private void OnGameplayInputPauseActionPerformed(object sender, EventArgs args) => RaisePauseGameRequested(sender);
		private void OnGameplayOpenInfoWindowRequested(object sender, OpenInfoWindowEventArgs args) => _gameplayUIState.OpenInfoWindow(args.Interactable);
		private void OnGameplayOpenContextMenuRequested(object sender, OpenContextMenuEventArgs args) => _gameplayUIState.OpenContextMenu(args.ContextMenuUser);
		private void OnGameplayCloseInfoWindowRequested(object sender, EventArgs args) => _gameplayUIState.CloseInfoWindow();
		private void OnGameplayCloseContextMenuRequested(object sender, EventArgs args) => _gameplayUIState.CloseContextMenu();

		#endregion

		#region Interactable Input Callbacks
		
		private void OnInteractionInputCancelActionPerformed(object sender, EventArgs args) => EndInteraction();
		
		#endregion

		#region Placement Mode Input Callbacks

		private void OnPlacementModeInputEndActionPerformed(object sender, EventArgs args) => EndPlacementMode();

		private void OnPlacementModeInputPreviewUpdated(object sender, WorldPositionEventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectPreviewRequest(args.ChunkPosition, args.TilePosition, _placementPrefab));

		private void OnPlacementModeInputPlacementRequested(object sender, WorldPositionEventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectPlacementRequest(args.ChunkPosition, args.TilePosition, _placementPrefab));

		private static void OnHidePlacementPreviewRequested(object sender, EventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectHidePreviewRequest());

		#endregion

		#region Global Messenger Callbacks

		private void OnBeginPlacementModeRequestReceived(BeginPlacementModeRequest message) => StartPlacementMode(message.PlacementPrefab);
		private void OnEndPlacementModeRequestReceived(EndPlacementModeRequest message) => EndPlacementMode();
		private void OnStartInteractionRequestReceived(StartInteractionRequest message) => StartInteraction(message.InteractionCallback);
		private void OnEndInteractionRequestReceived(EndInteractionRequest message) => EndInteraction();

		#endregion
	}
}