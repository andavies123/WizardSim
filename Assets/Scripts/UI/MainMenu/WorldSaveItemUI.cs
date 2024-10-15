using System;
using GameWorld;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.MainMenu
{
	public class WorldSaveItemUI : MonoBehaviour
	{
		[SerializeField, Required] private TMP_Text worldNameText;
		[SerializeField, Required] private TMP_Text dateCreatedText;
		[SerializeField, Required] private TMP_Text dateLastPlayedText;
		[SerializeField, Required] private Button deleteButton;

		private WorldSaveDetails _saveDetails;

		public event Action SaveDeleted;

		public void Initialize(WorldSaveDetails saveDetails)
		{
			_saveDetails = saveDetails;
			
			worldNameText.SetText(_saveDetails.name);
			dateCreatedText.SetText($"Created: {_saveDetails.dateCreated}");
			dateLastPlayedText.SetText($"Last Played: {_saveDetails.dateLastPlayed}");
		}
		
		private void OnDeleteButtonClicked()
		{
			WorldSaveUtility.DeleteSaveFolder(_saveDetails.saveId);
			SaveDeleted?.Invoke();
		}
		
		private void Awake()
		{
			deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		}

		private void OnDestroy()
		{
			deleteButton.onClick.RemoveAllListeners();
		}
	}
}