using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensions;
using TMPro;
using UnityEngine;
using Utilities.Attributes;

namespace UI.ContextMenus
{
	public class ContextMenu : MonoBehaviour
	{
		[Header("UI Elements")]
		[SerializeField, Required] private TMP_Text pathText;
		[SerializeField, Required] private RectTransform scaledTransform;
		
		[Header("Prefabs")]
		[SerializeField, Required] private ContextMenuNodePageUI contextMenuNodePagePrefab;
		[SerializeField, Required] private ContextMenuNodeUI contextMenuNodePrefab;

		[Header("Styles")]
		[SerializeField, Required] private ContextMenuStyling contextMenuStyling;
		
		private readonly List<ContextMenuTreeNode> _menuNodes = new();
		private ContextMenuNodePageUI _currentMenuNodePage;

		private List<(IContextMenuUser user, ContextMenuTreeNode rootNode)> _userTreePairs;
		private ContextMenuTreeNode _rootNode = null;
		
		private Vector2 _mouseClickScreenPosition;
		private float _groupWidth;
		private bool _isSubscribedToGroupEvents;

		public event EventHandler MenuClosed;

		private Vector3 CanvasScale => scaledTransform.localScale;

		public void Initialize(List<(IContextMenuUser, ContextMenuTreeNode)> userTreePairs, Vector2 screenPosition)
		{
			_userTreePairs = userTreePairs;
			
			_menuNodes.Clear();
			
			_mouseClickScreenPosition = screenPosition;
			pathText.rectTransform.position = CalculatePathTextPosition();
			BuildContextMenu();
		}
		
		public void CloseMenu()
		{
			_menuNodes.Clear();

			if (_currentMenuNodePage)
			{
				if (_isSubscribedToGroupEvents)
				{
					_currentMenuNodePage.ItemSelected -= OnItemSelected;
					_isSubscribedToGroupEvents = false;
				}
				
				_currentMenuNodePage.CleanUp();
			}
			
			MenuClosed?.Invoke(this, EventArgs.Empty);
		}
		
		public void NavigateUp() => _currentMenuNodePage.FocusedMenuItemIndex--;
		public void NavigateDown() => _currentMenuNodePage.FocusedMenuItemIndex++;
		public void NavigateForward() => GoForwardOneMenu(_currentMenuNodePage.FocusedMenuNodeUI.TreeNode);
		public void NavigateBack() => GoBackOneMenu();
		public void SelectCurrentItem() => SelectItem(_currentMenuNodePage.FocusedMenuNodeUI);

		private void BuildContextMenu()
		{
			if (_userTreePairs.IsNullOrEmpty())
				return;

			_rootNode = new ContextMenuTreeNode();
			_userTreePairs.ForEach(userNodePair =>
			{
				userNodePair.rootNode.ChildrenNodes.ForEach(childNode => _rootNode.AddChild(childNode));
			});

			if (!_currentMenuNodePage)
			{
				_currentMenuNodePage = Instantiate(contextMenuNodePagePrefab, transform);
				_currentMenuNodePage.Initialize(contextMenuStyling);
			}

			_currentMenuNodePage.GetComponent<RectTransform>().position = CalculateMenuPosition();
			
			if (!_isSubscribedToGroupEvents)
			{
				_currentMenuNodePage.ItemSelected += OnItemSelected;
				_isSubscribedToGroupEvents = true;
			}
			
			GoForwardOneMenu(_rootNode);
		}

		private void GoForwardOneMenu(ContextMenuTreeNode treeNode)
		{
			_menuNodes.Add(treeNode);
			UpdateCurrentMenuGroup();
		}

		private void GoBackOneMenu()
		{
			if (_menuNodes.TryRemoveLast(out _))
			{
				UpdateCurrentMenuGroup();
			}
		}

		private void UpdateCurrentMenuGroup()
		{
			if (!_currentMenuNodePage)
			{
				Debug.LogWarning("Unable to update context menu. UI unavailable...");
				return;
			}

			ContextMenuTreeNode currentTreeNode = _menuNodes.Last();

			if (currentTreeNode.IsLeafNode)
			{
				Debug.LogWarning("Unable to create context menu. Current menu item is a leaf node...");
				return;
			}
			
			_currentMenuNodePage.SetMenuItems(currentTreeNode.ChildrenNodes, _menuNodes.Count);
			UpdatePathText();
		}

		private void UpdatePathText()
		{
			StringBuilder pathBuilder = new();

			// Skip the ROOT text by starting the index at 1
			for (int index = 1; index < _menuNodes.Count; index++)
			{
				ContextMenuTreeNode menuNode = _menuNodes[index];
				pathBuilder.Append(menuNode.Text);

				if (index < _menuNodes.Count - 1)
				{
					pathBuilder.Append(" > ");
				}
			}

			pathText.SetText(pathBuilder.ToString());
		}

		private Vector3 CalculateMenuPosition()
		{
			/* Due to the canvas scalar, I found out that the _groupWidth value was not
			 being scaled unlike the position values. So we need to apply the scaling
			 to the width before continuing with our calculations */
			float scaledGroupWidth = _groupWidth * CanvasScale.x;
			bool buildRight = _mouseClickScreenPosition.x - scaledGroupWidth <= 0;
			
			// If I wanted to do the same thing for up/down I would need to know the height
			// that the context menu would be, so I would build upwards instead of down.
			
			// One issue I can see stemming from this is when the initial menu is small enough to be built
			// downwards, but if the next menu group is longer, it would go upwards and would confuse the player.
			
			float xPosition = _mouseClickScreenPosition.x - (buildRight ? 0 : scaledGroupWidth);
			float yPosition = _mouseClickScreenPosition.y;
			
			return new Vector3(xPosition, yPosition, 0);
		}

		private void SelectItem(ContextMenuNodeUI menuNodeUI)
		{
			switch (menuNodeUI.ItemType)
			{
				case ContextMenuItemType.Back:
					GoBackOneMenu();
					break;
				case ContextMenuItemType.Leaf:
					CloseMenu();
					menuNodeUI.TreeNode.MenuClickCallback?.Invoke(null/* Todo: Add correct IContextMenuUser here */);
					break;
				case ContextMenuItemType.Group:
					GoForwardOneMenu(menuNodeUI.TreeNode);
					break;
				default:
					throw new ArgumentOutOfRangeException(menuNodeUI.ItemType.ToString());
			}
		}

		private Vector3 CalculatePathTextPosition()
		{
			/* Due to the canvas scalar, I found out that the _groupWidth value was not
			 being scaled unlike the position values. So we need to apply the scaling
			 to the width before continuing with our calculations */
			float scaledGroupWidth = _groupWidth * CanvasScale.x;
			bool buildRight = _mouseClickScreenPosition.x - scaledGroupWidth <= 0;

			float xPosition = _mouseClickScreenPosition.x + _groupWidth/2 - (buildRight ? 0 : scaledGroupWidth);
			float yPosition = _mouseClickScreenPosition.y + 20; // Up 20
			
			return new Vector3(xPosition, yPosition, 0);
		}

		private void OnItemSelected(ContextMenuNodeUI menuNodeUI) => SelectItem(menuNodeUI);

		private void Awake()
		{
			_groupWidth = ((RectTransform) contextMenuNodePrefab.transform).rect.width;
		}
	}
}