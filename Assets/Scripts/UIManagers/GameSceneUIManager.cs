using Game;
using UnityEngine;

namespace UIManagers
{
	public class GameSceneUIManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		[SerializeField] private GameplayUIManager gameplayUIManager;
		[SerializeField] private PauseMenuUIManager pauseMenuUIManager;

		private UIManager _activeUIManager;

		public void ShowGameplayUI() => SetActiveUI(gameplayUIManager);
		public void ShowPauseMenuUI() => SetActiveUI(pauseMenuUIManager);

		private void Awake()
		{
			gameManager.GamePaused += OnGamePaused;
			gameManager.GameResumed += OnGameResumed;
			
			gameplayUIManager.PauseButtonPressed += OnPauseButtonPressed;
			pauseMenuUIManager.ResumeButtonPressed += OnResumeButtonPressed;
			pauseMenuUIManager.QuitButtonPressed += OnQuitButtonPressed;
		}

		private void Start()
		{
			gameplayUIManager.Disable();
			pauseMenuUIManager.Disable();
			
			ShowGameplayUI();
		}

		private void OnDestroy()
		{
			gameManager.GamePaused -= OnGamePaused;
			gameManager.GameResumed -= OnGameResumed;
			
			gameplayUIManager.PauseButtonPressed -= OnPauseButtonPressed;
			pauseMenuUIManager.ResumeButtonPressed -= OnResumeButtonPressed;
			pauseMenuUIManager.QuitButtonPressed -= OnQuitButtonPressed;
		}

		private void OnGamePaused() => ShowPauseMenuUI();
		private void OnGameResumed() => ShowGameplayUI();

		private void OnPauseButtonPressed() => gameManager.PauseGame();
		private void OnResumeButtonPressed() => gameManager.ResumeGame();
		private void OnQuitButtonPressed() => gameManager.QuitGame();

		private void SetActiveUI(UIManager uiManager)
		{
			if (_activeUIManager)
				_activeUIManager.Disable();

			_activeUIManager = uiManager;
			
			if (_activeUIManager)
				_activeUIManager.Enable();
		}
	}
}