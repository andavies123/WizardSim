using System.Collections.Generic;
using Extensions;
using GameWorld;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.MainMenu
{
	// Todo: Load World should show warning when deleting a world
	// Todo: Load World button should be disabled until a world is selected
	// Todo: Load World should update the date last played in the save
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MainMenuUIPage))]
	public class LoadGamePage : MonoBehaviour
	{
		[SerializeField, Required] private Button loadButton;
		[SerializeField, Required] private Button openFolderButton;
		[SerializeField, Required] private Button refreshListButton;
		
		[Header("Save Items")]
		[SerializeField, Required] private WorldSaveItemUI saveItemUIPrefab;
		[SerializeField, Required] private Transform saveItemContainer;
        
		private readonly List<WorldSaveItemUI> _saveItemUIs = new();
		private MainMenuUIPage _uiPage;

		private void DisplaySavedWorlds()
		{
			List<WorldSaveDetails> saveDetails = WorldSaveUtility.GetAllWorldSaves();
			
			saveDetails.ForEach(save =>
			{
				WorldSaveItemUI saveItemUI = Instantiate(saveItemUIPrefab, saveItemContainer);
				saveItemUI.SaveDeleted += OnSaveDeleted;
				saveItemUI.Initialize(save);
				_saveItemUIs.Add(saveItemUI);
			});
		}

		private void ClearSavedWorldUIs()
		{
			foreach (WorldSaveItemUI saveItem in _saveItemUIs)
			{
				saveItem.SaveDeleted -= OnSaveDeleted;
				saveItem.gameObject.Destroy();
			}

			_saveItemUIs.Clear();
		}

		private void OnPageEnabled()
		{
			loadButton.onClick.AddListener(OnLoadButtonClicked);
			openFolderButton.onClick.AddListener(OnOpenFolderButtonClicked);
			refreshListButton.onClick.AddListener(OnRefreshListButtonClicked);

			ClearSavedWorldUIs();
			DisplaySavedWorlds();
		}

		private void OnPageDisabled()
		{
			loadButton.onClick.RemoveAllListeners();
			openFolderButton.onClick.RemoveAllListeners();
			refreshListButton.onClick.RemoveAllListeners();
		}

		private void OnLoadButtonClicked()
		{
			print("Loading World not setup");
		}

		private void OnOpenFolderButtonClicked()
		{
			WorldSaveUtility.OpenSaveFolder();
		}

		private void OnRefreshListButtonClicked()
		{
			ClearSavedWorldUIs();
			DisplaySavedWorlds();
		}

		private void OnSaveDeleted()
		{
			ClearSavedWorldUIs();
			DisplaySavedWorlds();
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