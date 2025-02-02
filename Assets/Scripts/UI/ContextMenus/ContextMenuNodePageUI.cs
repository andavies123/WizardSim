using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using Utilities.Attributes;

namespace UI.ContextMenus
{
	[RequireComponent(typeof(RectTransform))]
	public class ContextMenuNodePageUI : MonoBehaviour
	{
		[SerializeField, Required] private ContextMenuNodeUI contextMenuNodePrefab;
		
		private readonly List<ContextMenuNodeUI> _menuNodeUIs = new();
		private readonly ContextMenuTreeNode _backMenuItem = new()
		{
			Text = "Previous",
			IsEnabledFunc = ContextMenuTreeNode.AlwaysTrue,
			IsVisibleFunc = ContextMenuTreeNode.AlwaysTrue,
			MenuClickCallback = null
		};

		private RectTransform _rectTransform;
		private ContextMenuUITreeMap _contextMenuUITreeMap;
		private ContextMenuStyling _styling = new();
		private int _focusedMenuItemIndex = 0;

		public event Action<ContextMenuNodeUI> ItemSelected;
		
		public ContextMenuNodeUI FocusedMenuNodeUI => _menuNodeUIs[FocusedMenuItemIndex];

		public int FocusedMenuItemIndex
		{
			get => _focusedMenuItemIndex;
			set
			{
				// This check is necessary since the previous value could have been
				// for a menu size larger than the current size
				if (_menuNodeUIs.IsValidIndex(_focusedMenuItemIndex))
				{
					_menuNodeUIs[_focusedMenuItemIndex].IsFocused = false;
				}
				
				_focusedMenuItemIndex = (value + _menuNodeUIs.Count) % _menuNodeUIs.Count;
				_menuNodeUIs[_focusedMenuItemIndex].IsFocused = true;
			}
		}

		public void Initialize(ContextMenuUITreeMap uiTreeMap, Vector3 menuPosition, ContextMenuStyling styling)
		{
			_contextMenuUITreeMap = uiTreeMap ?? throw new ArgumentNullException(nameof(uiTreeMap));
			_rectTransform.position = menuPosition;
			_styling = styling;
		}

		public void SetMenuItems(List<ContextMenuTreeNode> treeNodes, int treeIndex)
		{
			CleanUp();
			
			// Don't want to add the back option to the first group
			if (treeIndex > 1)
				_menuNodeUIs.Add(CreateMenuItemUI(_backMenuItem));
			
			// Loop through the rest
			foreach (ContextMenuTreeNode treeNode in treeNodes)
			{
				_menuNodeUIs.Add(CreateMenuItemUI(treeNode));
			}

			FocusedMenuItemIndex = 0;
		}
        
		public void CleanUp()
		{
			if (_menuNodeUIs.IsValidIndex(FocusedMenuItemIndex))
			{
				FocusedMenuNodeUI.IsFocused = false;
			}
			
			foreach (ContextMenuNodeUI menuItemUI in _menuNodeUIs)
			{
				menuItemUI.Selected -= OnContextMenuItemSelected;
				menuItemUI.FocusRequested -= OnContextMenuItemFocusRequested;
				Destroy(menuItemUI.gameObject);
			}
			
			_menuNodeUIs.Clear();
		}

		private ContextMenuNodeUI CreateMenuItemUI(ContextMenuTreeNode treeNode)
		{
			ContextMenuNodeUI menuNodeUI = Instantiate(contextMenuNodePrefab, transform);
			
			_contextMenuUITreeMap.TryGetUserFromNode(treeNode, out IContextMenuUser user);
            
			menuNodeUI.Initialize(treeNode, user, _styling, GetItemTypeFromMenuItem(treeNode));
			menuNodeUI.Selected += OnContextMenuItemSelected;
			menuNodeUI.FocusRequested += OnContextMenuItemFocusRequested;
			return menuNodeUI;
		}

		private ContextMenuItemType GetItemTypeFromMenuItem(ContextMenuTreeNode treeNode)
		{
			if (treeNode == _backMenuItem)
				return ContextMenuItemType.Back;

			if (treeNode.IsLeafNode)
				return ContextMenuItemType.Leaf;

			return ContextMenuItemType.Group;
		}

		private void OnContextMenuItemSelected(ContextMenuNodeUI menuNodeUI)
		{
			ItemSelected?.Invoke(menuNodeUI);
		}

		private void OnContextMenuItemFocusRequested(ContextMenuNodeUI menuNodeUI)
		{
			FocusedMenuItemIndex = _menuNodeUIs.IndexOf(menuNodeUI);
		}

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}
	}
}