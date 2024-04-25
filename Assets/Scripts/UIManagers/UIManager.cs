using System;
using Game;
using UnityEngine;

namespace UIManagers
{
	[DisallowMultipleComponent]
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private GameplayUIState gameplayUIState;
		[SerializeField] private PauseMenuUIState pauseMenuUIState;

		private UIState activeUIState;

		public void ShowGameplayUI() => SetActiveUI(gameplayUIState);
		public void ShowPauseMenuUI() => SetActiveUI(pauseMenuUIState);
		
		public void SetActiveUI(UIState uiState)
		{
			if (activeUIState)
				activeUIState.Disable();

			activeUIState = uiState;
			
			if (activeUIState)
				activeUIState.Enable();
		}

		private void Awake()
		{
			GameManager.Instance.GamePaused += OnGamePaused;
			GameManager.Instance.GameResumed += OnGameResumed;
			
			gameplayUIState.PauseButtonPressed += OnPauseButtonPressed;
			pauseMenuUIState.ResumeButtonPressed += OnResumeButtonPressed;
			pauseMenuUIState.QuitButtonPressed += OnQuitButtonPressed;
		}

		private void Start()
		{
			gameplayUIState.Disable();
			pauseMenuUIState.Disable();
			
			// Set the default ui state
			ShowGameplayUI();
		}

		private void OnDestroy()
		{
			GameManager.Instance.GamePaused -= OnGamePaused;
			GameManager.Instance.GameResumed -= OnGameResumed;
			
			gameplayUIState.PauseButtonPressed -= OnPauseButtonPressed;
			pauseMenuUIState.ResumeButtonPressed -= OnResumeButtonPressed;
			pauseMenuUIState.QuitButtonPressed -= OnQuitButtonPressed;
		}

		private void OnGamePaused(object sender, EventArgs args) => ShowPauseMenuUI();
		private void OnGameResumed(object sender, EventArgs args) => ShowGameplayUI();

		private static void OnPauseButtonPressed(object sender, EventArgs args) => GameManager.Instance.PauseGame();
		private static void OnResumeButtonPressed(object sender, EventArgs args) => GameManager.Instance.ResumeGame();
		private static void OnQuitButtonPressed(object sender, EventArgs args) => GameManager.Instance.QuitGame();
	}
}