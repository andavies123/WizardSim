using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;
using Utilities.Attributes;

namespace UI.ContextMenus
{
	public class ContextMenuNodeUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
	{
		[Header("UI Elements")]
		[SerializeField, Required] private TMP_Text itemText;
		[SerializeField, Required] private TMP_Text nextGroupArrow;
		[SerializeField, Required] private TMP_Text previousGroupArrow;
		[SerializeField, Required] private Image backgroundImage;

		private ContextMenuStyling _styling;
		private bool _isFocused = false;
		private bool _isEnabled = true;
		
		public event Action<ContextMenuNodeUI> Selected;
		public event Action<ContextMenuNodeUI> FocusRequested;
		
		public ContextMenuTreeNode TreeNode { get; private set; }
		public IContextMenuUser ContextMenuUser { get; private set; }
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
		
		public void Initialize(ContextMenuTreeNode treeNode, IContextMenuUser contextMenuUser, ContextMenuStyling styling, ContextMenuItemType itemType)
		{
			TreeNode = treeNode;
			ContextMenuUser = contextMenuUser;
			_styling = styling;
			ItemType = itemType;
			
			BuildUI();
			RecalculateVisibility();
		}

		// Todo: call this somewhere
		public void RecalculateVisibility()
		{
			if (ContextMenuUser != null)
				_isEnabled = TreeNode.IsEnabledFunc.Invoke(ContextMenuUser);
			UpdateBackgroundColor();
			UpdateTextColor();
		}
		
		public void OnPointerClick(PointerEventData eventData)
		{
			if (TreeNode == null || !_isEnabled)
				return;
			
			Selected?.Invoke(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			FocusRequested?.Invoke(this);
		}

		private void BuildUI()
		{
			if (TreeNode == null)
			{
				Debug.LogWarning($"Unable to build {nameof(ContextMenuNodeUI)}. {nameof(TreeNode)} is null...");
				return;
			}

			itemText.SetText(TreeNode.Text);

			switch (ItemType)
			{
				case ContextMenuItemType.Back: BuildAsBack(); break;
				case ContextMenuItemType.Leaf: BuildAsLeaf(); break;
				case ContextMenuItemType.Group: BuildAsGroup(); break;
				default: throw new ArgumentOutOfRangeException(ItemType.ToString());
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
				backgroundImage.color = _styling.FocusedBackgroundColor;
			else if (!_isEnabled)
				backgroundImage.color = _styling.DisabledBackgroundColor;
			else
				backgroundImage.color = _styling.DefaultBackgroundColor;
		}

		private void UpdateTextColor()
		{
			itemText.color = _isEnabled ? _styling.DefaultTextColor : _styling.DisabledTextColor;
		}
	}
}