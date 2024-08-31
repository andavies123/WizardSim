using System;
using CameraComponents;
using Game.GameStates;
using Game.GameStates.ContextMenuStates;
using Game.GameStates.GameplayStates;
using Game.GameStates.InteractionStates;
using Game.GameStates.PauseMenuStates;
using Game.GameStates.PlacementModeStates;
using Game.GameStates.TaskManagementGameStates;
using Game.MessengerSystem;
using InputStates.InputEventArgs;
using UI.ContextMenus;
using UI.Messages;
using UnityEngine;

namespace Game
{
	public class GameStateManager : MonoBehaviour
	{
		[Header("UI States")]
		[SerializeField] private GameplayUIState gameplayUIState;
		[SerializeField] private PauseMenuUIState pauseMenuUIState;
		[SerializeField] private InteractionUIState interactionUIState;
		[SerializeField] private ContextMenuUIState contextMenuUIState;
		[SerializeField] private PlacementModeUIState placementModeUIState;
		[SerializeField] private TaskManagementUIState taskManagementUIState;

		[Header("External Components")]
		[SerializeField] private InteractableRaycaster interactableRaycaster;

		private GameState _currentGameState;

		private GameplayGameState _gameplayGameState;
		private PauseMenuGameState _pauseMenuGameState;
		private InteractionGameState _interactionGameState;
		private ContextMenuGameState _contextMenuGameState;
		private PlacementModeGameState _placementModeGameState;
		private TaskManagementGameState _taskManagementGameState;

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
			_gameplayGameState = new GameplayGameState(gameplayUIState, interactableRaycaster);
			_pauseMenuGameState = new PauseMenuGameState(pauseMenuUIState);
			_interactionGameState = new InteractionGameState(interactionUIState, interactableRaycaster);
			_contextMenuGameState = new ContextMenuGameState(contextMenuUIState, interactableRaycaster);
			_placementModeGameState = new PlacementModeGameState(placementModeUIState, interactableRaycaster);
			_taskManagementGameState = new TaskManagementGameState(taskManagementUIState);
			
			_cameraInputState = new CameraInputState();
			Dependencies.RegisterDependency(_cameraInputState);
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
			
			// Set the initial game state
			UpdateCurrentState(_gameplayGameState);
		}

		private void OnEnable()
		{
			_gameplayGameState.PauseGameRequested += OnPauseGameRequested;
			_gameplayGameState.OpenContextMenuRequested += OnOpenContextMenuRequested;
			_gameplayGameState.BeginPlacementModeRequested += OnBeginPlacementModeRequested;
			_gameplayGameState.OpenTaskManagementWindow += OnOpenTaskManagementWindow;

			_pauseMenuGameState.ResumeGameRequested += OnResumeGameRequested;
			_pauseMenuGameState.QuitGameRequested += OnQuitGameRequested;

			_interactionGameState.InteractionCanceled += OnInteractionModeCanceled;

			_contextMenuGameState.MenuClosed += OnContextMenuClosed;

			_placementModeGameState.PlacementModeEnded += OnPlacementModeEnded;

			_taskManagementGameState.CloseMenu += OnCloseTaskManagementWindow;
			
			GlobalMessenger.Subscribe<StartInteractionRequest>(OnInteractionModeRequested);
			GlobalMessenger.Subscribe<EndInteractionRequest>(OnInteractionEndRequested);
		}

		private void OnDisable()
		{
			_gameplayGameState.PauseGameRequested -= OnPauseGameRequested;
			_gameplayGameState.OpenContextMenuRequested -= OnOpenContextMenuRequested;
			_gameplayGameState.BeginPlacementModeRequested -= OnBeginPlacementModeRequested;
			_gameplayGameState.OpenTaskManagementWindow -= OnOpenTaskManagementWindow;

			_pauseMenuGameState.ResumeGameRequested -= OnResumeGameRequested;
			_pauseMenuGameState.QuitGameRequested -= OnQuitGameRequested;

			_interactionGameState.InteractionCanceled -= OnInteractionModeCanceled;

			_contextMenuGameState.MenuClosed -= OnContextMenuClosed;
			
			_placementModeGameState.PlacementModeEnded -= OnPlacementModeEnded;
			
			_taskManagementGameState.CloseMenu -= OnCloseTaskManagementWindow;
			
			GlobalMessenger.Unsubscribe<StartInteractionRequest>(OnInteractionModeRequested);
			GlobalMessenger.Unsubscribe<EndInteractionRequest>(OnInteractionEndRequested);
		}

		// Pause Menu Related Events
		private void OnPauseGameRequested(object sender, EventArgs args) => UpdateCurrentState(_pauseMenuGameState);
		private void OnResumeGameRequested(object sender, EventArgs args) => UpdateCurrentState(_gameplayGameState);
		private static void OnQuitGameRequested(object sender, EventArgs args) => GameManager.Instance.QuitGame();

		// Context Menu Related Events
		private void OnOpenContextMenuRequested(object sender, (ContextMenuUser contextMenuUser, Vector2 screenPosition) args)
		{
			_contextMenuGameState.Initialize(args.contextMenuUser, args.screenPosition);
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
			_placementModeGameState.PlacementDetails = args.PlacementDetails;
			UpdateCurrentState(_placementModeGameState);
		}
		private void OnPlacementModeEnded(object sender, EventArgs args) => UpdateCurrentState(_gameplayGameState);

		// Interaction Mode Related Events
		private void OnInteractionModeRequested(StartInteractionRequest request)
		{
			_interactionGameState.SetInteractionCallback(request.InteractionCallback);
			UpdateCurrentState(_interactionGameState);
		}
		private void OnInteractionEndRequested(EndInteractionRequest request) => UpdateCurrentState(_gameplayGameState);
		private void OnInteractionModeCanceled(object sender, EventArgs args) => UpdateCurrentState(_gameplayGameState);
		
		// Task Management Menu Related Events
		private void OnOpenTaskManagementWindow(object sender, EventArgs args) => UpdateCurrentState(_taskManagementGameState);
		private void OnCloseTaskManagementWindow(object sender, EventArgs args) => UpdateCurrentState(_gameplayGameState);
	}
}