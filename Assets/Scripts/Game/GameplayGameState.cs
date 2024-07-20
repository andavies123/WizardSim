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
		private readonly InteractableRaycaster _interactableRaycaster;
		
		private readonly GameplayInputState gameplayInputState;
		private readonly SecondaryGameplayInputState secondaryGameplayInputState;
		private readonly InteractionInputState interactionInputState;
		private readonly PlacementModeInputState placementModeInputState;
		private readonly TaskManagementInputState taskManagementInputState;
		private readonly ContextMenuInputState contextMenuInputState;

		private GameObject _placementPrefab;
		
		public event EventHandler PauseGameRequested;
		
		public GameplayGameState(GameplayUIState gameplayUIState, InteractableRaycaster interactableRaycaster)
		{
			_gameplayUIState = gameplayUIState;
			_interactableRaycaster = interactableRaycaster;
			
			gameplayInputState = new GameplayInputState();
			secondaryGameplayInputState = new SecondaryGameplayInputState(_interactableRaycaster, _gameplayUIState);
			interactionInputState = new InteractionInputState(_interactableRaycaster);
			placementModeInputState = new PlacementModeInputState(_interactableRaycaster);
			taskManagementInputState = new TaskManagementInputState();
			contextMenuInputState = new ContextMenuInputState();
			
			Dependencies.RegisterDependency(gameplayInputState);
			Dependencies.RegisterDependency(secondaryGameplayInputState);
			Dependencies.RegisterDependency(interactionInputState);
			Dependencies.RegisterDependency(placementModeInputState);
			Dependencies.RegisterDependency(taskManagementInputState);
			Dependencies.RegisterDependency(contextMenuInputState);
		}

		public void Enable()
		{
			_gameplayUIState.PauseButtonPressed += OnUIPauseButtonPressed;
			_gameplayUIState.Enable();
			
			secondaryGameplayInputState.PauseActionPerformed += OnGameplayInputStatePauseActionPerformed;
			secondaryGameplayInputState.OpenInfoWindowRequested += OnGameplayOpenInfoWindowRequested;
			secondaryGameplayInputState.OpenContextMenuRequested += OnGameplayOpenContextMenuRequested;
			secondaryGameplayInputState.CloseInfoWindowRequested += OnGameplayCloseInfoWindowRequested;
			secondaryGameplayInputState.CloseContextMenuRequested += OnGameplayCloseContextMenuRequested;
			secondaryGameplayInputState.OpenTaskManagementRequested += OnGameplayOpenTaskManagementRequested;
			
			interactionInputState.CancelInteractionActionPerformed += OnInteractionInputStateCancelActionPerformed;
			
			placementModeInputState.EndPlacementModeActionPerformed += OnPlacementModeInputStateEndActionPerformed;
			placementModeInputState.PreviewPositionUpdated += OnPlacementModeInputStatePreviewUpdated;
			placementModeInputState.PlacementRequested += OnPlacementModeInputStatePlacementRequested;
			placementModeInputState.HidePlacementPreviewRequested += OnHidePlacementPreviewRequested;

			taskManagementInputState.CloseWindowRequested += OnTaskManagementCloseWindowRequested;

			contextMenuInputState.NavigationActionPerformed += OnContextMenuNavigationPerformed;
			contextMenuInputState.SelectActionPerformed += OnContextMenuSelectPerformed;
			contextMenuInputState.CloseActionPerformed += OnContextMenuClosePerformed;

			_gameplayUIState.ContextMenu.MenuOpened += OnContextMenuOpened;
			_gameplayUIState.ContextMenu.MenuClosed += OnContextMenuClosed;
			
			GlobalMessenger.Subscribe<BeginPlacementModeRequest>(OnBeginPlacementModeRequestReceived);
			GlobalMessenger.Subscribe<EndPlacementModeRequest>(OnEndPlacementModeRequestReceived);
			GlobalMessenger.Subscribe<StartInteractionRequest>(OnStartInteractionRequestReceived);
			GlobalMessenger.Subscribe<EndInteractionRequest>(OnEndInteractionRequestReceived);
			
			_mainInputStateMachine.SetCurrentState(gameplayInputState);
			_subInputStateMachine.SetCurrentState(secondaryGameplayInputState);
			_interactableRaycaster.enabled = true;
		}

		public void Disable()
		{
			_gameplayUIState.Disable();
			_gameplayUIState.PauseButtonPressed -= OnUIPauseButtonPressed;

			secondaryGameplayInputState.PauseActionPerformed -= OnGameplayInputStatePauseActionPerformed;
			secondaryGameplayInputState.OpenInfoWindowRequested -= OnGameplayOpenInfoWindowRequested;
			secondaryGameplayInputState.OpenContextMenuRequested -= OnGameplayOpenContextMenuRequested;
			secondaryGameplayInputState.CloseInfoWindowRequested -= OnGameplayCloseInfoWindowRequested;
			secondaryGameplayInputState.CloseContextMenuRequested -= OnGameplayCloseContextMenuRequested;
			secondaryGameplayInputState.OpenTaskManagementRequested -= OnGameplayOpenTaskManagementRequested;
			
			interactionInputState.CancelInteractionActionPerformed -= OnInteractionInputStateCancelActionPerformed;
			
			placementModeInputState.EndPlacementModeActionPerformed -= OnPlacementModeInputStateEndActionPerformed;
			placementModeInputState.PreviewPositionUpdated -= OnPlacementModeInputStatePreviewUpdated;
			placementModeInputState.PlacementRequested -= OnPlacementModeInputStatePlacementRequested;
			placementModeInputState.HidePlacementPreviewRequested -= OnHidePlacementPreviewRequested;
			
			taskManagementInputState.CloseWindowRequested -= OnTaskManagementCloseWindowRequested;
			
			contextMenuInputState.NavigationActionPerformed -= OnContextMenuNavigationPerformed;
			contextMenuInputState.SelectActionPerformed -= OnContextMenuSelectPerformed;
			contextMenuInputState.CloseActionPerformed -= OnContextMenuClosePerformed;
			
			_gameplayUIState.ContextMenu.MenuOpened -= OnContextMenuOpened;
			_gameplayUIState.ContextMenu.MenuClosed -= OnContextMenuClosed;
			
			GlobalMessenger.Unsubscribe<BeginPlacementModeRequest>(OnBeginPlacementModeRequestReceived);
			GlobalMessenger.Unsubscribe<EndPlacementModeRequest>(OnEndPlacementModeRequestReceived);
			GlobalMessenger.Unsubscribe<StartInteractionRequest>(OnStartInteractionRequestReceived);
			GlobalMessenger.Unsubscribe<EndInteractionRequest>(OnEndInteractionRequestReceived);
			
			_mainInputStateMachine.SetCurrentState(null);
			_interactableRaycaster.enabled = false;
		}

		private void StartInteraction(Action<MonoBehaviour> interactionCallback)
		{
			interactionInputState.InteractionCallback = interactionCallback;
			_subInputStateMachine.SetCurrentState(interactionInputState);
		}

		private void EndInteraction()
		{
			interactionInputState.InteractionCallback = null;
			_subInputStateMachine.SetCurrentState(secondaryGameplayInputState);
		}

		private void StartPlacementMode(GameObject placementPrefab)
		{
			_placementPrefab = placementPrefab;
			_subInputStateMachine.SetCurrentState(placementModeInputState);
		}

		private void EndPlacementMode()
		{
			_subInputStateMachine.SetCurrentState(secondaryGameplayInputState);
			_placementPrefab = null;
			GlobalMessenger.Publish(new PlacementModeEnded());
		}

		private void EnableTaskManagementWindow()
		{
			_gameplayUIState.OpenTaskManagementWindow();
			_mainInputStateMachine.SetCurrentState(null);
			_subInputStateMachine.SetCurrentState(taskManagementInputState);
			_interactableRaycaster.enabled = false;
		}

		private void DisableTaskManagementWindow()
		{
			_gameplayUIState.CloseTaskManagementWindow();
			_mainInputStateMachine.SetCurrentState(gameplayInputState);
			_subInputStateMachine.SetCurrentState(secondaryGameplayInputState);
			_interactableRaycaster.enabled = true;
		}

		private void StartContextMenuMode()
		{
			_mainInputStateMachine.SetCurrentState(null);
			_subInputStateMachine.SetCurrentState(contextMenuInputState);
		}

		private void EndContextMenuMode()
		{
			_mainInputStateMachine.SetCurrentState(gameplayInputState);
			_subInputStateMachine.SetCurrentState(secondaryGameplayInputState);
		}
		
		private void RaisePauseGameRequested(object sender)
		{
			PauseGameRequested?.Invoke(sender, EventArgs.Empty);
		}

		private void OnUIPauseButtonPressed(object sender, EventArgs args)
		{
			RaisePauseGameRequested(sender);
		}

		#region Secondary Gameplay Input Callbacks

		private void OnGameplayInputStatePauseActionPerformed(object sender, EventArgs args) => RaisePauseGameRequested(sender);
		private void OnGameplayOpenInfoWindowRequested(object sender, OpenInfoWindowEventArgs args) => _gameplayUIState.OpenInfoWindow(args.Interactable);
		private void OnGameplayOpenContextMenuRequested(object sender, OpenContextMenuEventArgs args) => _gameplayUIState.OpenContextMenu(args.ContextMenuUser, args.ScreenPosition);
		private void OnGameplayCloseInfoWindowRequested(object sender, EventArgs args) => _gameplayUIState.CloseInfoWindow();
		private void OnGameplayCloseContextMenuRequested(object sender, EventArgs args) => _gameplayUIState.ContextMenu.CloseMenu();
		private void OnGameplayOpenTaskManagementRequested(object sender, EventArgs args) => EnableTaskManagementWindow();

		#endregion

		#region Interactable Input Callbacks
		
		private void OnInteractionInputStateCancelActionPerformed(object sender, EventArgs args) => EndInteraction();

		#endregion

		#region Placement Mode Input Callbacks

		private void OnPlacementModeInputStateEndActionPerformed(object sender, EventArgs args) => EndPlacementMode();

		private void OnPlacementModeInputStatePreviewUpdated(object sender, WorldPositionEventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectPreviewRequest(args.ChunkPosition, args.TilePosition, _placementPrefab));

		private void OnPlacementModeInputStatePlacementRequested(object sender, WorldPositionEventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectPlacementRequest(args.ChunkPosition, args.TilePosition, _placementPrefab));

		private static void OnHidePlacementPreviewRequested(object sender, EventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectHidePreviewRequest());

		#endregion

		#region Task Management Input Callbacks

		private void OnTaskManagementCloseWindowRequested(object sender, EventArgs args) => DisableTaskManagementWindow();

		#endregion
		
		#region Context Menu Input Callbacks

		private void OnContextMenuNavigationPerformed(object sender, ContextMenuNavigationEventArgs args)
		{
			switch (args.NavigationOption)
			{
				case NavigationOption.Up:
					_gameplayUIState.ContextMenu.NavigateUp();
					break;
				case NavigationOption.Down: 
					_gameplayUIState.ContextMenu.NavigateDown();
					break;
				case NavigationOption.Left:
                    _gameplayUIState.ContextMenu.NavigateBack();
					break;
				case NavigationOption.Right:
					_gameplayUIState.ContextMenu.NavigateForward();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(args.NavigationOption), args.NavigationOption, "Enum case does not exist");
			}
		}

		private void OnContextMenuSelectPerformed(object sender, EventArgs args) => _gameplayUIState.ContextMenu.SelectCurrentItem();
		private void OnContextMenuClosePerformed(object sender, EventArgs args) => _gameplayUIState.ContextMenu.CloseMenu();
		
		#endregion
		
		#region UI Callbacks

		private void OnContextMenuOpened(object sender, EventArgs args) => StartContextMenuMode();
		private void OnContextMenuClosed(object sender, EventArgs args) => EndContextMenuMode();
        
		#endregion

		#region Global Messenger Callbacks

		private void OnBeginPlacementModeRequestReceived(BeginPlacementModeRequest message) => StartPlacementMode(message.PlacementPrefab);
		private void OnEndPlacementModeRequestReceived(EndPlacementModeRequest message) => EndPlacementMode();
		private void OnStartInteractionRequestReceived(StartInteractionRequest message) => StartInteraction(message.InteractionCallback);
		private void OnEndInteractionRequestReceived(EndInteractionRequest message) => EndInteraction();

		#endregion
	}
}