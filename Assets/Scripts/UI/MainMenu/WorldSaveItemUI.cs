using System;
using GameWorld;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.MainMenu
{
	public class WorldSaveItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField, Required] private TMP_Text worldNameText;
		[SerializeField, Required] private TMP_Text dateCreatedText;
		[SerializeField, Required] private TMP_Text dateLastPlayedText;
		[SerializeField, Required] private Button deleteButton;

		[Header("Selection Color Tints")]
		[SerializeField] private Image backgroundImage;
		[SerializeField] private Color hoverTint;
		[SerializeField] private Color selectedTint;

		private WorldSaveDetails _saveDetails;
		private Color _originalColor;
		private bool _isSelected;
		private bool _isHovered;

		public event Action SaveDeleted;
		public event Action<WorldSaveItemUI> Selected;
		public event Action<WorldSaveItemUI> Deselected;

		public void Initialize(WorldSaveDetails saveDetails)
		{
			_saveDetails = saveDetails;
			
			worldNameText.SetText(_saveDetails.name);
			dateCreatedText.SetText($"Created: {_saveDetails.dateCreated}");
			dateLastPlayedText.SetText($"Last Played: {_saveDetails.dateLastPlayed}");
		}

		public void Select()
		{
			_isSelected = true;
			backgroundImage.color = selectedTint;
			UpdateBackgroundColor();
		}

		public void Deselect()
		{
			_isSelected = false;
			backgroundImage.color = _originalColor;
			UpdateBackgroundColor();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			if (_isSelected)
				Deselected?.Invoke(this);
			else
				Selected?.Invoke(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_isHovered = true;
			UpdateBackgroundColor();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_isHovered = false;
			UpdateBackgroundColor();
		}

		private void UpdateBackgroundColor()
		{
			if (_isSelected)
				backgroundImage.color = _originalColor * selectedTint;
			else if (_isHovered)
				backgroundImage.color = _originalColor * hoverTint;
			else
				backgroundImage.color = _originalColor;
		}
		
		private void OnDeleteButtonClicked()
		{
			WorldSaveUtility.DeleteSaveFolder(_saveDetails.saveId);
			SaveDeleted?.Invoke();
		}
		
		private void Awake()
		{
			_originalColor = backgroundImage.color;
			
			deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		}

		private void OnDestroy()
		{
			deleteButton.onClick.RemoveAllListeners();
		}
	}
}