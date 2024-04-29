using System;
using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenuInteractions : MonoBehaviour
	{
		[SerializeField] private Interactable interactable;
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
			if (interactable)
				interactable.SecondaryActionSelected += OnInteractableSecondaryActionSelected;
		
			if (contextMenuUser)
				contextMenuUser.MenuClosed += OnContextMenuClosed;
		}
		
		private void OnDestroy()
		{
			if (interactable)
				interactable.SecondaryActionSelected -= OnInteractableSecondaryActionSelected;
		
			if (contextMenuUser != null)
				contextMenuUser.MenuClosed -= OnContextMenuClosed;
		}

		private void OnInteractableSecondaryActionSelected(object sender, EventArgs args)
		{
			if (!contextMenuUser)
				return;
			
			contextMenuUser.OpenMenu();
			IsContextMenuOpen = true;
		}
		
		private void OnContextMenuClosed(object sender, EventArgs args)
		{
			IsContextMenuOpen = false;
		}
	}
}