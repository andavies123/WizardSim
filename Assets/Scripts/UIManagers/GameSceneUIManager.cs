using Game;
using UnityEngine;

namespace UIManagers
{
	public class GameSceneUIManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		[SerializeField] private GameplayUIManager gameplayUIManager;
		[SerializeField] private PauseMenuUIManager pauseMenuUIManager;

		public void ShowGameplayUI()
		{
			gameplayUIManager.gameObject.SetActive(true);
			pauseMenuUIManager.gameObject.SetActive(false);
		}

		public void ShowPauseMenuUI()
		{
			gameplayUIManager.gameObject.SetActive(false);
			pauseMenuUIManager.gameObject.SetActive(true);
		}

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
	}
}