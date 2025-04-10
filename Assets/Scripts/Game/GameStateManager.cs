﻿using System;
using CameraComponents;
using Game.Events;
using Game.GameStates;
using Game.GameStates.ContextMenuStates;
using Game.GameStates.GameplayStates;
using Game.GameStates.InteractionStates;
using Game.GameStates.PauseMenuStates;
using Game.GameStates.PlacementModeStates;
using Game.GameStates.TaskManagementGameStates;
using Game.GameStates.TownManagementStates;
using UI.ContextMenus;
using UI.Upgrades;
using UnityEngine;
using Utilities.Attributes;

namespace Game
{
	public class GameStateManager : MonoBehaviour
	{
		[Header("UI States")]
		[SerializeField, Required] private GameplayUIState gameplayUIState;
		[SerializeField, Required] private PauseMenuUIState pauseMenuUIState;
		[SerializeField, Required] private InteractionUIState interactionUIState;
		[SerializeField, Required] private ContextMenuUIState contextMenuUIState;
		[SerializeField, Required] private PlacementModeUIState placementModeUIState;
		[SerializeField, Required] private TaskManagementUIState taskManagementUIState;
		[SerializeField, Required] private TownManagementUIState townManagementUIState;

		[Header("UI Windows")]
		[SerializeField, Required] private UpgradeScreen upgradeScreen;

		[Header("External Components")]
		[SerializeField, Required] private SelectionManager selectionManager;
		[SerializeField, Required] private InteractableRaycaster interactableRaycaster;

		private GameState _currentGameState;

		private GameplayGameState _gameplayGameState;
		private PauseMenuGameState _pauseMenuGameState;
		private InteractionGameState _interactionGameState;
		private ContextMenuGameState _contextMenuGameState;
		private PlacementModeGameState _placementModeGameState;
		private TaskManagementGameState _taskManagementGameState;
		private TownManagementGameState _townManagementGameState;

		private CameraInputState _cameraInputState;

		public void UpdateCurrentState(GameState nextGameState)
		{
			_currentGameState?.Disable();
			_currentGameState = nextGameState;

			if (_currentGameState != null)
			{
				_currentGameState.Enable();
				
				// Todo: Also set up the interactions enabling/disabling 
				if (_currentGameState.AllowCameraInputs && !_cameraInputState.IsEnabled)
				{
                    _cameraInputState.Enable();
				}
				else if (!_currentGameState.AllowCameraInputs && _cameraInputState.IsEnabled)
				{
					_cameraInputState.Disable();
				}
			}
		}

		private void Awake()
		{
			_gameplayGameState = new GameplayGameState(gameplayUIState, selectionManager);
			_pauseMenuGameState = new PauseMenuGameState(pauseMenuUIState);
			_interactionGameState = new InteractionGameState(interactionUIState, interactableRaycaster);
			_contextMenuGameState = new ContextMenuGameState(contextMenuUIState, interactableRaycaster);
			_placementModeGameState = new PlacementModeGameState(placementModeUIState, interactableRaycaster);
			_taskManagementGameState = new TaskManagementGameState(taskManagementUIState);
			_townManagementGameState = new TownManagementGameState(townManagementUIState);
			
			_cameraInputState = new CameraInputState();
			Dependencies.Register(_cameraInputState);
		}

		private void Start()
		{
			// Disable all game states
			_gameplayGameState.Disable();
			_pauseMenuGameState.Disable();
			_interactionGameState.Disable();
			_contextMenuGameState.Disable();
			_placementModeGameState.Disable();
			_taskManagementGameState.Disable();
			_townManagementGameState.Disable();
			
			// Set the initial game state
			UpdateCurrentState(_gameplayGameState);
		}

		private void OnEnable()
		{
			_gameplayGameState.PauseGameRequested += OnPauseGameRequested;
			_gameplayGameState.OpenContextMenuRequested += OnOpenContextMenuRequested;
			_gameplayGameState.BeginPlacementModeRequested += OnBeginPlacementModeRequested;
			_gameplayGameState.OpenTaskManagementWindow += OnOpenTaskManagementWindow;
			_gameplayGameState.OpenTownManagementWindow += OnOpenTownManagementWindow;

			_interactionGameState.InteractionCanceled += OnInteractionModeCanceled;

			_contextMenuGameState.MenuClosed += OnContextMenuClosed;

			_placementModeGameState.PlacementModeEnded += OnPlacementModeEnded;

			_taskManagementGameState.CloseMenu += OnCloseTaskManagementWindow;

			_townManagementGameState.CloseMenu += OnCloseTownManagementWindow;

			GameEvents.General.GamePaused.Raised += OnGamePaused;
			GameEvents.General.GameResumed.Raised += OnGameResumed;

			GameEvents.UI.StartInteraction.Requested += OnStartInteractionRequested;
			GameEvents.UI.EndInteraction.Requested += OnEndInteractionRequested;
			GameEvents.UI.OpenUI.Requested += OnOpenUIRequested;
			GameEvents.UI.CloseUI.Requested += OnCloseUIRequested;
		}

		private void OnDisable()
		{
			_gameplayGameState.PauseGameRequested -= OnPauseGameRequested;
			_gameplayGameState.OpenContextMenuRequested -= OnOpenContextMenuRequested;
			_gameplayGameState.BeginPlacementModeRequested -= OnBeginPlacementModeRequested;
			_gameplayGameState.OpenTaskManagementWindow -= OnOpenTaskManagementWindow;
			_gameplayGameState.OpenTownManagementWindow -= OnOpenTownManagementWindow;

			_interactionGameState.InteractionCanceled -= OnInteractionModeCanceled;

			_contextMenuGameState.MenuClosed -= OnContextMenuClosed;
			
			_placementModeGameState.PlacementModeEnded -= OnPlacementModeEnded;
			
			_taskManagementGameState.CloseMenu -= OnCloseTaskManagementWindow;

			_townManagementGameState.CloseMenu -= OnCloseTownManagementWindow;

			GameEvents.General.GamePaused.Raised -= OnGamePaused;
			GameEvents.General.GameResumed.Raised -= OnGameResumed;

			GameEvents.UI.StartInteraction.Requested -= OnStartInteractionRequested;
			GameEvents.UI.EndInteraction.Requested -= OnEndInteractionRequested;
			GameEvents.UI.OpenUI.Requested -= OnOpenUIRequested;
			GameEvents.UI.CloseUI.Requested -= OnCloseUIRequested;
		}

		// Pause Menu Related Events
		private void OnPauseGameRequested(object sender, EventArgs args) => GameEvents.General.PauseGame.Request(this);

		// Context Menu Related Events
		private void OnOpenContextMenuRequested(IContextMenuUser[] contextMenuUsers)
		{
			_contextMenuGameState.Initialize(contextMenuUsers);
			UpdateCurrentState(_contextMenuGameState);
		}
		private void OnContextMenuClosed(object sender, EventArgs args)
		{
			// Todo: Update the event args to say whether or not interaction mode is needed
			// There's a possibility that the context menu will force the interaction state
			if (_currentGameState == _contextMenuGameState)
				UpdateCurrentState(_gameplayGameState);
		}

		// Placement Mode Related Events
		private void OnBeginPlacementModeRequested(object sender, BeginPlacementModeEventArgs args)
		{
			_placementModeGameState.LatestPreviewWorldObject = args.WorldObjectDetails;
			UpdateCurrentState(_placementModeGameState);
		}
		private void OnPlacementModeEnded(object sender, EventArgs args) => UpdateCurrentState(_gameplayGameState);

		// Interaction Mode Related Events
		private void OnStartInteractionRequested(object sender, StartInteractionEventArgs args)
		{
			_interactionGameState.SetInteractionCallback(args.InteractionCallback);
			UpdateCurrentState(_interactionGameState);
		}
		
		private void OnEndInteractionRequested(object sender, EventArgs args)
		{
			UpdateCurrentState(_gameplayGameState);
		}

		private void OnInteractionModeCanceled(object sender, EventArgs args) => UpdateCurrentState(_gameplayGameState);
		
		// Task Management Menu Related Events
		private void OnOpenTaskManagementWindow(object sender, EventArgs args) => UpdateCurrentState(_taskManagementGameState);
		private void OnCloseTaskManagementWindow(object sender, EventArgs args) => UpdateCurrentState(_gameplayGameState);

		// Town Management Menu Related Events
		private void OnOpenTownManagementWindow(object sender, EventArgs args) => UpdateCurrentState(_townManagementGameState);
		private void OnCloseTownManagementWindow(object sender, EventArgs args) => UpdateCurrentState(_gameplayGameState);

		// We should be able to pause the game from any state
		private void OnGamePaused(object sender, EventArgs args) => UpdateCurrentState(_pauseMenuGameState);
		private void OnGameResumed(object sender, EventArgs args) => UpdateCurrentState(_gameplayGameState);
		
		private void OnOpenUIRequested(object sender, OpenUIEventArgs args)
		{
			switch (args.Window)
			{
				case UIWindow.UpgradeWindow:
					upgradeScreen.Activate();
					break;
				case UIWindow.TownHallWindow:
				default: break;
			}
		}
		
		private void OnCloseUIRequested(object sender, CloseUIEventArgs args)
		{
			switch (args.Window)
			{
				case UIWindow.UpgradeWindow:
					upgradeScreen.Deactivate();
					break;
				case UIWindow.TownHallWindow:
				default: break;
			}
		}
	}
}