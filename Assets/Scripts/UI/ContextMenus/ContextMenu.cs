using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensions;
using TMPro;
using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenu : MonoBehaviour
	{
		[Header("UI Elements")]
		[SerializeField] private TMP_Text pathText;
		
		[Header("Prefabs")]
		[SerializeField] private ContextMenuItemGroupUI contextMenuItemGroupPrefab;
		[SerializeField] private ContextMenuItemUI contextMenuItemPrefab;
		
		private readonly List<ContextMenuItem> _menuItems = new();
		private ContextMenuItemGroupUI _currentMenuItemGroup;
		private ContextMenuUser _contextMenuUser;
		
		private Vector2 _mouseClickScreenPosition;
		private float _groupWidth = 0;
		private bool _isSubscribedToGroupEvents = false;

		public event EventHandler MenuClosed;

		public bool IsOpen { get; private set; }

		public void OpenMenu(ContextMenuUser user, Vector2 screenPosition)
		{
			_menuItems.Clear();
            
			if (_contextMenuUser)
			{
				_contextMenuUser.IsOpen = false;
			}

			_contextMenuUser = user;
			_contextMenuUser.IsOpen = true;
			_mouseClickScreenPosition = screenPosition;
			pathText.rectTransform.position = CalculatePathTextPosition();
			BuildContextMenu();
			gameObject.SetActive(true);
			IsOpen = true;
		}
		
		public void CloseMenu()
		{
			IsOpen = false;
			gameObject.SetActive(false);
			
			if (_contextMenuUser)
			{
				_contextMenuUser.IsOpen = false;
				_contextMenuUser = null;
			}
			
			_menuItems.Clear();

			if (_isSubscribedToGroupEvents)
			{
				_currentMenuItemGroup.LeafItemSelected -= OnLeafMenuItemSelected;
				_currentMenuItemGroup.GroupItemSelected -= OnGroupMenuItemSelected;
				_currentMenuItemGroup.BackItemSelected -= OnBackMenuItemSelected;
				_isSubscribedToGroupEvents = false;
			}
			
			_currentMenuItemGroup.CleanUp();
			
			MenuClosed?.Invoke(this, EventArgs.Empty);
		}

		private void BuildContextMenu()
		{
			if (!_contextMenuUser || _contextMenuUser.MenuItemTree.IsEmpty)
			{
				return;
			}

			if (!_currentMenuItemGroup)
			{
				_currentMenuItemGroup = Instantiate(contextMenuItemGroupPrefab, transform);
			}

			_currentMenuItemGroup.GetComponent<RectTransform>().position = CalculateMenuPosition();
			
			if (!_isSubscribedToGroupEvents)
			{
				_currentMenuItemGroup.LeafItemSelected += OnLeafMenuItemSelected;
				_currentMenuItemGroup.GroupItemSelected += OnGroupMenuItemSelected;
				_currentMenuItemGroup.BackItemSelected += OnBackMenuItemSelected;
				_isSubscribedToGroupEvents = true;
			}
			
			GoForwardOneMenu(_contextMenuUser.MenuItemTree.RootMenuItem);
		}

		private void GoForwardOneMenu(ContextMenuItem menuItem)
		{
			_menuItems.Add(menuItem);
			UpdateCurrentMenuGroup();
		}

		private void GoBackOneMenu()
		{
			if (_menuItems.TryRemoveLast(out _))
			{
				UpdateCurrentMenuGroup();
			}
		}

		private void UpdateCurrentMenuGroup()
		{
			if (!_currentMenuItemGroup)
			{
				Debug.LogWarning("Unable to update context menu. UI unavailable...");
				return;
			}

			ContextMenuItem currentMenuItem = _menuItems.Last();

			if (currentMenuItem.IsLeaf)
			{
				Debug.LogWarning("Unable to create context menu. Current menu item is a leaf node...");
				return;
			}
			
			_currentMenuItemGroup.Initialize(currentMenuItem.ChildMenuItems, _menuItems.Count);
			UpdatePathText();
		}

		private void UpdatePathText()
		{
			StringBuilder pathBuilder = new();

			// Skip the ROOT text by starting the index at 1
			for (int index = 1; index < _menuItems.Count; index++)
			{
				ContextMenuItem menuItem = _menuItems[index];
				pathBuilder.Append(menuItem.Name);

				if (index < _menuItems.Count - 1)
				{
					pathBuilder.Append(" > ");
				}
			}

			pathText.SetText(pathBuilder.ToString());
		}

		private Vector3 CalculateMenuPosition()
		{
			bool buildRight = _mouseClickScreenPosition.x - _groupWidth <= 0;
			
			// If I wanted to do the same thing for up/down I would need to know the height
			// that the context menu would be, so I would build upwards instead of down.
			
			// One issue I can see stemming from this is when the initial menu is small enough to be built
			// downwards but if the next menu group is longer, it would go upwards and would confuse the player.
			
			// This would have to be something I would want to play around with when I don't have
			// any changes, so I could revert easier.
			
			float xPosition = _mouseClickScreenPosition.x + (buildRight ? 0 : -_groupWidth);
			float yPosition = _mouseClickScreenPosition.y;
			
			return new Vector3(xPosition, yPosition, 0);
		}

		private Vector3 CalculatePathTextPosition()
		{
			bool buildRight = _mouseClickScreenPosition.x - _groupWidth <= 0;

			float xPosition = _mouseClickScreenPosition.x + _groupWidth/2 + (buildRight ? 0 : -_groupWidth);
			float yPosition = _mouseClickScreenPosition.y + 10;

			return new Vector3(xPosition, yPosition, 0);
		}

		private void OnLeafMenuItemSelected(ContextMenuItemUI menuItemUI)
		{
			menuItemUI.ContextMenuItem.MenuClickCallback?.Invoke();
			CloseMenu();
		}

		private void OnGroupMenuItemSelected(ContextMenuItemUI menuItemUI)
		{
			while (_menuItems.Count > menuItemUI.TreeIndex + 1)
			{
				GoBackOneMenu();
			}
			GoForwardOneMenu(menuItemUI.ContextMenuItem);
		}

		private void OnBackMenuItemSelected()
		{
			GoBackOneMenu();
		}

		private void Awake()
		{	
			contextMenuItemPrefab.ThrowIfNull(nameof(contextMenuItemPrefab));
			contextMenuItemGroupPrefab.ThrowIfNull(nameof(contextMenuItemGroupPrefab));

			_groupWidth = ((RectTransform) contextMenuItemPrefab.transform).rect.width;
		}
	}
}