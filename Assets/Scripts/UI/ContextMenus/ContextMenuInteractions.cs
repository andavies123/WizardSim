using System;
using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenuInteractions : MonoBehaviour
	{
		[SerializeField] private MouseInteractionEvents mouseInteractionEvents;
		[SerializeField] private ContextMenuUser contextMenuUser;

		private bool _isContextMenuOpen = false;
		
		public event Action<bool> IsContextMenuOpenValueChanged;

		public bool IsContextMenuOpen
		{
			get => _isContextMenuOpen;
			private set
			{
				if (_isContextMenuOpen == value)
					return;

				_isContextMenuOpen = value;
				IsContextMenuOpenValueChanged?.Invoke(_isContextMenuOpen);
			}
		}

		private void Awake()
		{
			if (mouseInteractionEvents != null)
				mouseInteractionEvents.RightMousePressed += OnRightMousePressed;
		
			if (contextMenuUser != null)
				contextMenuUser.MenuClosed += OnContextMenuClosed;
		}
		
		private void OnDestroy()
		{
			if (mouseInteractionEvents != null)
				mouseInteractionEvents.RightMousePressed -= OnRightMousePressed;
		
			if (contextMenuUser != null)
				contextMenuUser.MenuClosed -= OnContextMenuClosed;
		}

		private void OnRightMousePressed()
		{
			if (contextMenuUser == null)
				return;
			
			contextMenuUser.OpenMenu();
			IsContextMenuOpen = true;
		}
		
		private void OnContextMenuClosed()
		{
			IsContextMenuOpen = false;
		}
	}
}