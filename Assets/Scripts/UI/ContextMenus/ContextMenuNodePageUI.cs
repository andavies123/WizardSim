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
			{
				ContextMenuNodeUI menuNodeUI = CreateMenuNodeUI(_backMenuItem);
				if (menuNodeUI)
					_menuNodeUIs.Add(menuNodeUI);
			}
			
			// Loop through the rest
			foreach (ContextMenuTreeNode treeNode in treeNodes)
			{
				ContextMenuNodeUI menuNodeUI = CreateMenuNodeUI(treeNode);
				if (menuNodeUI)
					_menuNodeUIs.Add(menuNodeUI);
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

		private ContextMenuNodeUI CreateMenuNodeUI(ContextMenuTreeNode treeNode)
		{
			// Its ok if this is null, we won't always have a user (group nodes/back)
			_contextMenuUITreeMap.TryGetUserFromNode(treeNode, out IContextMenuUser user);
			
			if (!treeNode.IsVisibleFunc.Invoke(user))
				return null;
            
			ContextMenuNodeUI menuNodeUI = Instantiate(contextMenuNodePrefab, transform);
			menuNodeUI.Initialize(treeNode, user, _styling, GetItemTypeFromMenuItem(treeNode));
			menuNodeUI.Selected += OnContextMenuItemSelected;
			menuNodeUI.FocusRequested += OnContextMenuItemFocusRequested;
			return menuNodeUI;
		}

		private ContextMenuNodeType GetItemTypeFromMenuItem(ContextMenuTreeNode treeNode)
		{
			if (treeNode == _backMenuItem)
				return ContextMenuNodeType.Back;

			if (treeNode.IsLeafNode)
				return ContextMenuNodeType.Leaf;

			return ContextMenuNodeType.Group;
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