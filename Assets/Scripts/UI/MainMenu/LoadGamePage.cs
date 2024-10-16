using System;
using System.Collections.Generic;
using System.Globalization;
using Extensions;
using GameWorld;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.MainMenu
{
	// Todo: Load World should show warning when deleting a world
	// Todo: Load World should load the world on the gameplay scene
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
		private WorldSaveItemUI _selectedSave;
		private MainMenuUIPage _uiPage;

		private bool IsSaveSelected => _selectedSave;

		private void DisplaySavedWorlds()
		{
			List<WorldSaveDetails> saveDetails = WorldSaveUtility.GetAllWorldSaves();
			
			saveDetails.ForEach(save =>
			{
				WorldSaveItemUI saveItemUI = Instantiate(saveItemUIPrefab, saveItemContainer);
				saveItemUI.Selected += OnSaveSelected;
				saveItemUI.Deselected += OnSaveDeselected;
				saveItemUI.SaveDeleted += OnSaveDeleted;
				saveItemUI.Initialize(save);
				_saveItemUIs.Add(saveItemUI);
			});
		}

		private void ClearSavedWorldUIs()
		{
			foreach (WorldSaveItemUI saveItem in _saveItemUIs)
			{
				saveItem.Selected -= OnSaveSelected;
				saveItem.Deselected -= OnSaveDeselected;
				saveItem.SaveDeleted -= OnSaveDeleted;
				saveItem.gameObject.Destroy();
			}

			_saveItemUIs.Clear();
		}

		private void ValidateInputs()
		{
			loadButton.interactable = IsSaveSelected;
		}

		private void OnPageEnabled()
		{
			loadButton.onClick.AddListener(OnLoadButtonClicked);
			openFolderButton.onClick.AddListener(OnOpenFolderButtonClicked);
			refreshListButton.onClick.AddListener(OnRefreshListButtonClicked);

			ClearSavedWorldUIs();
			DisplaySavedWorlds();
			ValidateInputs();
		}

		private void OnPageDisabled()
		{
			loadButton.onClick.RemoveAllListeners();
			openFolderButton.onClick.RemoveAllListeners();
			refreshListButton.onClick.RemoveAllListeners();
		}

		private void OnLoadButtonClicked()
		{
			// Update the last played date
			_selectedSave.SaveDetails.dateLastPlayed = DateTime.Now.ToString(CultureInfo.CurrentCulture);
			WorldSaveUtility.UpdateSaveDetails(_selectedSave.SaveDetails);
			
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

		private void OnSaveSelected(WorldSaveItemUI selectedSave)
		{
			if (_selectedSave)
				_selectedSave.Deselect();
			
			_selectedSave = selectedSave;

			if (_selectedSave)
				_selectedSave.Select();
			
			ValidateInputs();
		}

		private void OnSaveDeselected(WorldSaveItemUI selectedSave)
		{
			if (_selectedSave != selectedSave)
				return;

			_selectedSave.Deselect();
			_selectedSave = null;
			ValidateInputs();
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