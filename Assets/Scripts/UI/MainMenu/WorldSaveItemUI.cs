using System;
using GameWorld;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.Attributes;
using static UI.MainMenu.PopupToken;

namespace UI.MainMenu
{
	public class WorldSaveItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		private const string POPUP_MESSAGE = "Are you sure you want to delete this save?";
		private const string POPUP_ACCEPT_TEXT = "Already made up my mind!";
		private const string POPUP_REJECT_TEXT = "Maybe I'll hold on to it for now.";
        
		[SerializeField, Required] private TMP_Text worldNameText;
		[SerializeField, Required] private TMP_Text dateCreatedText;
		[SerializeField, Required] private TMP_Text dateLastPlayedText;
		[SerializeField, Required] private Button deleteButton;

		[Header("Selection Color Tints")]
		[SerializeField] private Image backgroundImage;
		[SerializeField] private Color hoverTint;
		[SerializeField] private Color selectedTint;

		private readonly PopupToken _popupToken = new();
		private PopupUI _popupUI;
		private Color _originalColor;
		private bool _isSelected;
		private bool _isHovered;

		public event Action SaveDeleted;
		public event Action<WorldSaveItemUI> Selected;
		public event Action<WorldSaveItemUI> Deselected;
		
		public WorldSaveDetails SaveDetails { get; private set; }

		public void Initialize(WorldSaveDetails saveDetails, PopupUI popupUI)
		{
			SaveDetails = saveDetails;
			_popupUI = popupUI;
			
			worldNameText.SetText(SaveDetails.name);
			dateCreatedText.SetText($"Created: {SaveDetails.dateCreated}");
			dateLastPlayedText.SetText($"Last Played: {SaveDetails.dateLastPlayed}");
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

		private void DeleteSave()
		{
			WorldSaveUtility.DeleteSaveFolder(SaveDetails.saveId);
			SaveDeleted?.Invoke();
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
			_popupUI.Initialize(_popupToken, POPUP_MESSAGE, POPUP_ACCEPT_TEXT, POPUP_REJECT_TEXT);
		}

		private void OnPopupClosed(CloseType closeType)
		{
			if (closeType == CloseType.Accepted)
				DeleteSave();
		}
		
		private void Awake()
		{
			_originalColor = backgroundImage.color;
		}

		private void OnEnable()
		{
			_popupToken.PopupClosed += OnPopupClosed;
			deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		}

		private void OnDisable()
		{
			_popupToken.PopupClosed -= OnPopupClosed;
			print("Disable");
			deleteButton.onClick.RemoveAllListeners();
		}
	}
}