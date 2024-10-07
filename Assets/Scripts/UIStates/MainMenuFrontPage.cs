using System;
using Game.GameStates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UIStates
{
	public class MainMenuFrontPage : UIState
	{
		[SerializeField] private string gameplaySceneName;
		
		[Header("UI Elements")]
		[SerializeField, Required] private Button playButton;
		[SerializeField, Required] private Button optionsButton;
		[SerializeField, Required] private Button quitButton;

		public event Action OptionsButtonPressed;

		protected override void OnStateEnabled()
		{
			playButton.onClick.AddListener(OnPlayButtonPressed);
			optionsButton.onClick.AddListener(OnOptionsButtonPressed);
			quitButton.onClick.AddListener(OnQuitButtonPressed);
		}

		protected override void OnStateDisabled()
		{
			playButton.onClick.RemoveListener(OnPlayButtonPressed);
			optionsButton.onClick.AddListener(OnOptionsButtonPressed);
			quitButton.onClick.RemoveListener(OnQuitButtonPressed);
		}

		private void OnOptionsButtonPressed() => OptionsButtonPressed?.Invoke();
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