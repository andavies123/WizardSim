using System.Linq;
using Game.GameStates;
using UIStates;
using UnityEngine;
using Utilities.Attributes;

namespace UI.MainMenu
{
	public class MainMenuUIManager : MonoBehaviour
	{
		[Header("Front Page")]
		[SerializeField, Required] private MainMenuFrontPage frontPage;
		
		[Header("Play Pages")]
		[SerializeField, Required] private MainMenuPlayPage playPage;
		[SerializeField, Required] private MainMenuNewGamePage newGamePage;
		
		[Header("Option Pages")]
		[SerializeField, Required] private MainMenuOptionsPage optionsPage;

		private UIState _activeUI;
		
		private void Start()
		{
			// Front page events
			frontPage.PlayButtonPressed += OnFrontPagePlayPressed;
			frontPage.OptionsButtonPressed += OnFrontPageOptionsPressed;
			
			// Play page events
			playPage.BackButtonPressed += OnPlayPageBackPressed;
			playPage.NewGameButtonPressed += OnPlayPageNewGamePressed;
			playPage.LoadGameButtonPressed += OnPlayPageLoadGamePressed;
			
			// New Game page events
			newGamePage.BackButtonPressed += OnNewGamePageBackPressed;
			
			// Options page events
			optionsPage.BackButtonPressed += OnOptionsPageBackPressed;

			// Disable all UI states that are children of this object
			foreach (UIState uiState in GetComponentsInChildren<UIState>().ToList())
			{
				uiState.Disable();
			}
			
			// Enable the front page
			ChangeUI(frontPage);
		}

		private void OnDestroy()
		{
			// Front page events
			frontPage.PlayButtonPressed -= OnFrontPagePlayPressed;
			frontPage.OptionsButtonPressed -= OnFrontPageOptionsPressed;
			
			// Play page events
			playPage.BackButtonPressed -= OnPlayPageBackPressed;
			playPage.NewGameButtonPressed -= OnPlayPageNewGamePressed;
			playPage.LoadGameButtonPressed -= OnPlayPageLoadGamePressed;
			
			// New Game page events
			newGamePage.BackButtonPressed -= OnNewGamePageBackPressed;
			
			// Options page events
			optionsPage.BackButtonPressed -= OnOptionsPageBackPressed;
		}

		private void ChangeUI(UIState newState)
		{
			if (_activeUI) _activeUI.Disable();
			_activeUI = newState;
			if (_activeUI) _activeUI.Enable();
		}

		// Front Page Callbacks
		private void OnFrontPagePlayPressed() => ChangeUI(playPage);
		private void OnFrontPageOptionsPressed() => ChangeUI(optionsPage);

		// Play Page Callbacks
		private void OnPlayPageBackPressed() => ChangeUI(frontPage);
		//private void OnPlayPageNewGamePressed() => SceneManager.LoadScene("GameplayScene");
		private void OnPlayPageNewGamePressed() => ChangeUI(newGamePage);
		private void OnPlayPageLoadGamePressed() { }
		
		// New Game Page Callbacks
		private void OnNewGamePageBackPressed() => ChangeUI(playPage);

		// Options Page Callbacks
		private void OnOptionsPageBackPressed() => ChangeUI(frontPage);
	}
}
