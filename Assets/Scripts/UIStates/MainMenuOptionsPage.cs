using System;
using Game.GameStates;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UIStates
{
	public class MainMenuOptionsPage : UIState
	{
		[Header("UI Elements")]
		[SerializeField, Required] private Button backButton;

		public event Action BackButtonPressed;
		
		protected override void OnStateEnabled()
		{
			backButton.onClick.AddListener(OnBackButtonPressed);
		}

		protected override void OnStateDisabled()
		{
			backButton.onClick.RemoveListener(OnBackButtonPressed);
		}

		private void OnBackButtonPressed() => BackButtonPressed?.Invoke();
	}
}