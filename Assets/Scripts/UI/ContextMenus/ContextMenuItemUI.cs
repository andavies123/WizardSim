using System;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;

namespace UI.ContextMenus
{
	public class ContextMenuItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
	{
		
		[Header("UI Elements")]
		[SerializeField] private TMP_Text itemText;
		[SerializeField] private TMP_Text nextGroupArrow;
		[SerializeField] private TMP_Text previousGroupArrow;
		[SerializeField] private Image backgroundImage;

		private ContextMenuStyling _styling;
		private bool _isFocused = false;
		
		public event Action<ContextMenuItemUI> Selected;
		public event Action<ContextMenuItemUI> FocusRequested;
		
		public ContextMenuItem ContextMenuItem { get; private set; }
		public ContextMenuItemType ItemType { get; private set; }

		public bool IsFocused
		{
			get => _isFocused;
			set
			{
				_isFocused = value;
				UpdateBackgroundColor();
			}
		}
		
		public void Initialize(ContextMenuItem contextMenuItem, ContextMenuStyling styling, ContextMenuItemType itemType)
		{
			ContextMenuItem = contextMenuItem;
			_styling = styling;
			ItemType = itemType;
			
			BuildUI();
			contextMenuItem.RecalculateVisibility();
			UpdateBackgroundColor();
			UpdateTextColor();
		}
		
		public void OnPointerClick(PointerEventData eventData)
		{
			if (ContextMenuItem == null)
				return;

			if (!ContextMenuItem.IsEnabled)
				return;
			
			Selected?.Invoke(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			FocusRequested?.Invoke(this);
		}

		private void BuildUI()
		{
			if (ContextMenuItem == null)
			{
				Debug.LogWarning($"Unable to build {nameof(ContextMenuItemUI)}. {nameof(ContextMenuItem)} is null...");
				return;
			}

			itemText.SetText(ContextMenuItem.Name);

			if (ContextMenuItem.IsBack)
			{
				BuildAsBack();
			}
			else if (ContextMenuItem.IsLeaf)
			{
				BuildAsLeaf();
			}
			else
			{
				BuildAsGroup();
			}	
		}

		private void BuildAsLeaf()
		{
			nextGroupArrow.gameObject.SetActive(false);
			previousGroupArrow.gameObject.SetActive(false);
			itemText.alignment = TextAlignmentOptions.Left;
			itemText.rectTransform.SetAnchor(AnchorOption.StretchLeft);
		}

		private void BuildAsGroup()
		{
			nextGroupArrow.gameObject.SetActive(true);
			previousGroupArrow.gameObject.SetActive(false);
			itemText.alignment = TextAlignmentOptions.Left;
			itemText.rectTransform.SetAnchor(AnchorOption.StretchLeft);
		}

		private void BuildAsBack()
		{
			nextGroupArrow.gameObject.SetActive(false);
			previousGroupArrow.gameObject.SetActive(true);
			itemText.alignment = TextAlignmentOptions.Right;
			itemText.rectTransform.SetAnchor(AnchorOption.StretchRight);
		}

		private void UpdateBackgroundColor()
		{
			if (IsFocused)
			{
				backgroundImage.color = _styling.FocusedBackgroundColor;
			}
			else if (!ContextMenuItem.IsEnabled)
			{
				backgroundImage.color = _styling.DisabledBackgroundColor;
			}
			else
			{
				backgroundImage.color = _styling.DefaultBackgroundColor;
			}
		}

		private void UpdateTextColor()
		{
			itemText.color = ContextMenuItem.IsEnabled ? _styling.DefaultTextColor : _styling.DisabledTextColor;
		}

		private void Awake()
		{
			itemText.ThrowIfNull(nameof(itemText));
			nextGroupArrow.ThrowIfNull(nameof(nextGroupArrow));
			previousGroupArrow.ThrowIfNull(nameof(previousGroupArrow));
			backgroundImage.ThrowIfNull(nameof(backgroundImage));
		}
	}
}