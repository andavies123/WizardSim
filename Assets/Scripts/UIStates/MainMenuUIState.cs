using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIStates
{
	public class MainMenuUIState : UIState
	{
		[SerializeField] private string gameplaySceneName;
		
		[Header("UI Elements")]
		[SerializeField] private Button playButton;
		[SerializeField] private Button quitButton;

		protected override void OnStateEnabled()
		{
			playButton.onClick.AddListener(OnPlayButtonPressed);
			quitButton.onClick.AddListener(OnQuitButtonPressed);
		}

		protected override void OnStateDisabled()
		{
			playButton.onClick.RemoveListener(OnPlayButtonPressed);
			quitButton.onClick.RemoveListener(OnQuitButtonPressed);
		}

		private void OnPlayButtonPressed() => SceneManager.LoadScene(gameplaySceneName);
		
		private static void OnQuitButtonPressed()
		{
			#if UNITY_STANDALONE
			Application.Quit();
			#endif
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
		}
	}
}