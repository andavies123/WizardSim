using System.Linq;
using Game.GameStates;
using UIStates;
using UnityEngine;
using Utilities.Attributes;

namespace UI.MainMenu
{
	public class MainMenuUIManager : MonoBehaviour
	{
		[Header("UI States")]
		[SerializeField, Required] private MainMenuFrontPage frontPage;
		[SerializeField, Required] private MainMenuOptionsPage optionsPage;
		
		private void Start()
		{
			// Front page events
			frontPage.OptionsButtonPressed += OnFrontPageOptionsPressed;
			
			// Options page events
			optionsPage.BackButtonPressed += OnOptionsPageBackPressed;

			// Disable all UI states that are children of this object
			foreach (UIState uiState in GetComponentsInChildren<UIState>().ToList())
			{
				uiState.Disable();
			}
			
			// Enable the front page
			frontPage.Enable();
		}

		private void OnFrontPageOptionsPressed()
		{
			frontPage.Disable();
			optionsPage.Enable();
		}

		private void OnOptionsPageBackPressed()
		{
			optionsPage.Disable();
			frontPage.Enable();
		}
	}
}
