using Game.Events;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.MainMenu
{
	public class MainMenuFrontPage : MonoBehaviour
	{
		[SerializeField, Required] private Button quitButton;

		private void OnEnable()
		{
			quitButton.onClick.AddListener(OnQuitButtonPressed);
		}

		private void OnDisable()
		{
			quitButton.onClick.RemoveListener(OnQuitButtonPressed);
		}

		private void OnQuitButtonPressed()
		{
			GameEvents.General.CloseGame.Request(this);
		}
	}
}