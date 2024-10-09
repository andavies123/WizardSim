using System;
using Game.GameStates;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UIStates
{
	public class MainMenuPlayPage : UIState
	{
		[Header("UI Elements")]
		[SerializeField, Required] private Button backButton;
		[SerializeField, Required] private Button newGameButton;
		[SerializeField, Required] private Button loadGameButton;
		
		public event Action BackButtonPressed;
		public event Action NewGameButtonPressed;
		public event Action LoadGameButtonPressed;
		
		protected override void OnStateEnabled()
		{
			backButton.onClick.AddListener(OnBackButtonPressed);
			newGameButton.onClick.AddListener(OnNewGameButtonPressed);
			loadGameButton.onClick.AddListener(OnLoadGameButtonPressed);
		}

		protected override void OnStateDisabled()
		{
			backButton.onClick.RemoveListener(OnBackButtonPressed);
			newGameButton.onClick.RemoveListener(OnNewGameButtonPressed);
			loadGameButton.onClick.RemoveListener(OnLoadGameButtonPressed);
		}

		private void OnBackButtonPressed() => BackButtonPressed?.Invoke();
		private void OnNewGameButtonPressed() => NewGameButtonPressed?.Invoke();
		private void OnLoadGameButtonPressed() => LoadGameButtonPressed?.Invoke();
	}
}