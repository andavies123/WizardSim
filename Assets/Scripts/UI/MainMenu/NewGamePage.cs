using GameWorld;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.MainMenu
{
	// Todo: Create the world
	// Todo: Create a save folder for the new world
	
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MainMenuUIPage))]
	public class NewGamePage : MonoBehaviour
	{
		[SerializeField, Required] private TMP_InputField worldNameInput;
		[SerializeField, Required] private TMP_InputField worldSeedInput;
		[SerializeField, Required] private Button createButton;

		private MainMenuUIPage _uiPage;

		private bool IsWorldNameValid => worldNameInput.text.Length > 0;
		private bool IsWorldSeedValid => worldSeedInput.text.Length > 0;
		
		private void OnPageEnabled()
		{
			worldNameInput.SetTextWithoutNotify(string.Empty);
			worldSeedInput.SetTextWithoutNotify(string.Empty);
			ValidateAllInputs();

			worldNameInput.onValueChanged.AddListener(_ => ValidateAllInputs());
			worldSeedInput.onValueChanged.AddListener(_ => ValidateAllInputs());
			createButton.onClick.AddListener(OnCreateButtonClicked);
		}

		private void OnPageDisabled()
		{
			worldNameInput.onValueChanged.RemoveAllListeners();
			worldSeedInput.onValueChanged.RemoveAllListeners();
			createButton.onClick.RemoveAllListeners();
		}

		private void ValidateAllInputs() => createButton.interactable = IsWorldNameValid && IsWorldSeedValid;

		private void OnCreateButtonClicked()
		{
			// Static class so its available while changing scenes
			StartGameplayDetails.CreateWorld(worldNameInput.text, worldSeedInput.text);

			SceneManager.LoadScene("GameplayScene");
		}

		private void Awake()
		{
			_uiPage = GetComponent<MainMenuUIPage>();

			_uiPage.PageEnabled += OnPageEnabled;
			_uiPage.PageDisabled += OnPageDisabled;
		}

		private void OnDestroy()
		{
			_uiPage.PageEnabled -= OnPageEnabled;
			_uiPage.PageDisabled -= OnPageDisabled;
		}
	}
}