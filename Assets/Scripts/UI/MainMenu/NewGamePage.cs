using GameWorld;
using PersistantManagers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.MainMenu
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MainMenuUIPage))]
	public class NewGamePage : MonoBehaviour
	{
		[SerializeField, Required] private TMP_InputField worldNameInput;
		[SerializeField, Required] private TMP_InputField worldSeedInput;
		[SerializeField, Required] private Button createButton;
		[SerializeField, Required] private WorldLoadDetails loadDetails;

		private MainMenuUIPage _uiPage;

		private string WorldName => worldNameInput.text;
		private string WorldSeed => worldSeedInput.text;
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
			NewWorldDetails details = new(WorldName, WorldSeed);
			loadDetails.InitializeAsNewWorld(details);

			WorldSaveUtility.CreateNewSaveFolder(WorldName);	
			
			// Todo: I should probably go down the event channel for changing scenes.
			// Todo: I would have to make sure this scene gets unloaded as well
			SceneManager.LoadScene(SceneNames.GAMEPLAY_SCENE_NAME);
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