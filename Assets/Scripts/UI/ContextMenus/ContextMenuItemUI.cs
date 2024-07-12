using System;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.ContextMenus
{
	public class ContextMenuItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[FormerlySerializedAs("itemName")]
		[Header("UI Elements")]
		[SerializeField] private TMP_Text itemText;
		[SerializeField] private TMP_Text nextGroupArrow;
		[SerializeField] private TMP_Text previousGroupArrow;
		[SerializeField] private Image backgroundImage;

		[Header("Background Colors")]
		[SerializeField] private Color defaultBackgroundColor;
		[SerializeField] private Color hoverBackgroundColor;
		[SerializeField] private Color disabledBackgroundColor;
		
		[Header("Text Colors")]
		[SerializeField] private Color defaultTextColor;
		[SerializeField] private Color disabledTextColor;

		private bool _isHovered = false;
		
		public event Action<ContextMenuItemUI> MenuItemSelected;
		
		public ContextMenuItem ContextMenuItem { get; private set; }
		public int TreeIndex { get; private set; }

		private bool IsHovered
		{
			get => _isHovered;
			set
			{
				if (value != _isHovered)
				{
					_isHovered = value;
					UpdateBackgroundColor();
				}
			}
		}
		
		public void Initialize(ContextMenuItem contextMenuItem, int treeIndex)
		{
			ContextMenuItem = contextMenuItem;
			TreeIndex = treeIndex;
			
			contextMenuItem.RecalculateVisibility();
			BuildUI();
			UpdateBackgroundColor();
			UpdateTextColor();
		}
		
		public void OnPointerClick(PointerEventData eventData)
		{
			if (ContextMenuItem == null)
				return;

			if (!ContextMenuItem.IsEnabled)
				return;
			
			MenuItemSelected?.Invoke(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			IsHovered = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			IsHovered = false;
		}

		private void BuildUI()
		{
			if (ContextMenuItem == null)
				return;

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
		}

		private void BuildAsGroup()
		{
			nextGroupArrow.gameObject.SetActive(true);
			previousGroupArrow.gameObject.SetActive(false);
			itemText.alignment = TextAlignmentOptions.Left;
		}

		private void BuildAsBack()
		{
			nextGroupArrow.gameObject.SetActive(false);
			previousGroupArrow.gameObject.SetActive(true);
			itemText.alignment = TextAlignmentOptions.Right;
		}

		private void UpdateBackgroundColor()
		{
			if (!ContextMenuItem.IsEnabled)
			{
				backgroundImage.color = disabledBackgroundColor;
			}
			else if (IsHovered)
			{
				backgroundImage.color = hoverBackgroundColor;
			}
			else
			{
				backgroundImage.color = defaultBackgroundColor;
			}
		}

		private void UpdateTextColor()
		{
			if (ContextMenuItem.IsEnabled)
			{
				itemText.color = defaultTextColor;
			}
			else
			{
				itemText.color = disabledTextColor;
			}
		}

		private void Awake()
		{
			itemText.ThrowIfNull(nameof(itemText));
			nextGroupArrow.ThrowIfNull(nameof(nextGroupArrow));
			backgroundImage.ThrowIfNull(nameof(backgroundImage));
		}
	}
}