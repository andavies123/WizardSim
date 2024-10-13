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
		[SerializeField, Required] private Button deleteButton;

		private WorldSaveQuickDetails _saveDetails;

		public event Action SaveDeleted;

		public void Initialize(WorldSaveQuickDetails saveDetails)
		{
			_saveDetails = saveDetails;
			
			worldNameText.SetText(_saveDetails.Name);
		}
		
		private void OnDeleteButtonClicked()
		{
			WorldSaveUtility.DeleteSaveFolder(_saveDetails.SaveId);
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