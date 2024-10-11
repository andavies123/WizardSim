using TMPro;
using UnityEngine;
using Utilities.Attributes;

namespace UI.MainMenu
{
	// Todo: Disable create button until all input fields are filled out
	// Todo: Create an object to store all world creation variables in
	// Todo: Start the gameplay scene
	// Todo: Create the world
	// Todo: Create a save folder for the new world
	
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MainMenuUIPage))]
	public class NewGamePage : MonoBehaviour
	{
		[SerializeField, Required] private TMP_InputField worldNameInput;
		[SerializeField, Required] private TMP_InputField worldSeedInput;

		private MainMenuUIPage _uiPage;
		
		private void OnPageEnabled()
		{
			worldNameInput.SetTextWithoutNotify(string.Empty);
			worldSeedInput.SetTextWithoutNotify(string.Empty);
		}

		private void Awake()
		{
			_uiPage = GetComponent<MainMenuUIPage>();

			_uiPage.PageEnabled += OnPageEnabled;
		}

		private void OnDestroy()
		{
			_uiPage.PageEnabled -= OnPageEnabled;
		}
	}
}