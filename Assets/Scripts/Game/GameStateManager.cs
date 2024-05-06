using System;
using CameraComponents;
using UI;
using UIStates;
using UnityEngine;

namespace Game
{
	public class GameStateManager : MonoBehaviour
	{
		[Header("UI States")]
		[SerializeField] private GameplayUIState gameplayUIState;
		[SerializeField] private PauseMenuUIState pauseMenuUIState;

		[Header("Components")]
		[SerializeField] private InteractableRaycaster interactableRaycaster;
		[SerializeField] private InteractionEvents interactionEvents;

		private IGameState _currentGameState;
		
		public GameplayGameState GameplayGameState { get; private set; }
		public PauseGameState PauseGameState { get; private set; }

		public void UpdateCurrentState(IGameState newGameState)
		{
			_currentGameState?.Disable();
			_currentGameState = newGameState;
			_currentGameState?.Enable();
		}

		private void Awake()
		{
			GameplayGameState = new GameplayGameState(gameplayUIState, interactableRaycaster, interactionEvents);
			PauseGameState = new PauseGameState(pauseMenuUIState);
		}

		private void Start()
		{
			GameplayGameState.Disable();
			PauseGameState.Disable();
			
			UpdateCurrentState(GameplayGameState);
		}

		private void OnEnable()
		{
			PauseGameState.ResumeGameRequested += OnPauseStateResumeGameRequested;
			PauseGameState.QuitGameRequested += OnPauseStateQuitGameRequested;

			GameplayGameState.PauseGameRequested += OnGameplayStatePauseGameRequested;
		}

		private void OnDisable()
		{
			PauseGameState.ResumeGameRequested -= OnPauseStateResumeGameRequested;
			PauseGameState.QuitGameRequested -= OnPauseStateQuitGameRequested;

			GameplayGameState.PauseGameRequested -= OnGameplayStatePauseGameRequested;
		}

		private static void OnPauseStateQuitGameRequested(object sender, EventArgs args) => GameManager.Instance.QuitGame();
		
		private void OnPauseStateResumeGameRequested(object sender, EventArgs args)
		{
			UpdateCurrentState(GameplayGameState);
			GameManager.Instance.ResumeGame();
		}

		private void OnGameplayStatePauseGameRequested(object sender, EventArgs args)
		{
			GameManager.Instance.PauseGame();
			UpdateCurrentState(PauseGameState);
		}
	}
}