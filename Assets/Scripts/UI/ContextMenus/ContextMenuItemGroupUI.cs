using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace UI.ContextMenus
{
	[RequireComponent(typeof(RectTransform))]
	public class ContextMenuItemGroupUI : MonoBehaviour
	{
		[SerializeField] private ContextMenuItemUI contextMenuItemPrefab;
		
		private readonly List<ContextMenuItemUI> _menuItemUIs = new();
		private readonly ContextMenuItem _backMenuItem = new("Previous")
		{
			IsEnabledFunc = ContextMenuItem.AlwaysTrue,
			IsVisibleFunc = ContextMenuItem.AlwaysTrue,
			MenuClickCallback = null,
			IsBack = true
		};

		private ContextMenuStyling _styling = new();
		private int _focusedMenuItemIndex = 0;

		public event Action<ContextMenuItemUI> ItemSelected;
		
		public RectTransform RectTransform { get; private set; }
		public ContextMenuItemUI FocusedMenuItemUI => _menuItemUIs[FocusedMenuItemIndex];

		public int FocusedMenuItemIndex
		{
			get => _focusedMenuItemIndex;
			set
			{
				// This check is necessary since the previous value could have been
				// for a menu size larger than the current size
				if (_menuItemUIs.IsValidIndex(_focusedMenuItemIndex))
				{
					_menuItemUIs[_focusedMenuItemIndex].IsFocused = false;
				}
				
				_focusedMenuItemIndex = (value + _menuItemUIs.Count) % _menuItemUIs.Count;
				_menuItemUIs[_focusedMenuItemIndex].IsFocused = true;
			}
		}

		public void Initialize(ContextMenuStyling styling)
		{
			_styling = styling;
		}

		public void SetMenuItems(List<ContextMenuItem> menuItems, int treeIndex)
		{
			CleanUp();
			
			// Don't want to add the back option to the first group
			if (treeIndex > 1)
			{
				_menuItemUIs.Add(CreateMenuItemUI(_backMenuItem));
			}
			
			// Loop through the rest
			foreach (ContextMenuItem menuItem in menuItems)
			{
				_menuItemUIs.Add(CreateMenuItemUI(menuItem));
			}

			FocusedMenuItemIndex = 0;
		}
        
		public void CleanUp()
		{
			if (_menuItemUIs.IsValidIndex(FocusedMenuItemIndex))
			{
				FocusedMenuItemUI.IsFocused = false;
			}
			
			foreach (ContextMenuItemUI menuItemUI in _menuItemUIs)
			{
				menuItemUI.Selected -= OnContextMenuItemSelected;
				menuItemUI.FocusRequested -= OnContextMenuItemFocusRequested;
				Destroy(menuItemUI.gameObject);
			}
			
			_menuItemUIs.Clear();
		}

		private ContextMenuItemUI CreateMenuItemUI(ContextMenuItem menuItem)
		{
			ContextMenuItemUI menuItemUI = Instantiate(contextMenuItemPrefab, transform);
			menuItemUI.Initialize(menuItem, _styling, GetItemTypeFromMenuItem(menuItem));
			menuItemUI.Selected += OnContextMenuItemSelected;
			menuItemUI.FocusRequested += OnContextMenuItemFocusRequested;
			return menuItemUI;
		}

		private ContextMenuItemType GetItemTypeFromMenuItem(ContextMenuItem menuItem)
		{
			if (menuItem == _backMenuItem)
				return ContextMenuItemType.Back;

			if (menuItem.IsLeaf)
				return ContextMenuItemType.Leaf;

			return ContextMenuItemType.Group;
		}

		private void OnContextMenuItemSelected(ContextMenuItemUI menuItemUI)
		{
			ItemSelected?.Invoke(menuItemUI);
		}

		private void OnContextMenuItemFocusRequested(ContextMenuItemUI menuItemUI)
		{
			FocusedMenuItemIndex = _menuItemUIs.IndexOf(menuItemUI);
		}

		private void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
		}
	}
}