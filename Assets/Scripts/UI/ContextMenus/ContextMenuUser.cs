using System;
using Game.MessengerSystem;
using UI.Messages;
using UnityEngine;

namespace UI.ContextMenus
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Interactable))]
	public class ContextMenuUser : MonoBehaviour
	{
		private bool _isOpen = false;
		
		public event EventHandler IsContextMenuOpenValueChanged;

		public ContextMenuItemTree MenuItemTree { get; } = new();
		public Interactable Interactable { get; private set; }

		public bool IsOpen
		{
			get => _isOpen;
			set
			{
				if (_isOpen == value)
					return;

				_isOpen = value;
				IsContextMenuOpenValueChanged?.Invoke(this, EventArgs.Empty);
			}
		}
        
		public void AddMenuItem(string path, Action menuClickCallback, Func<bool> isEnabledFunc, Func<bool> isVisibleFunc)
		{   
			MenuItemTree.AddChildMenuItem(path, menuClickCallback, isEnabledFunc, isVisibleFunc);
		}

		private void Awake()
		{
			Interactable = GetComponent<Interactable>();
		}

		private void Start()
		{
			Interactable.SecondaryActionSelected += OnInteractableContextMenuOpened;
		}

		private void OnDestroy()
		{
			Interactable.SecondaryActionSelected -= OnInteractableContextMenuOpened;
		}
		
		private void OnInteractableContextMenuOpened(object sender, EventArgs args)
		{
			GlobalMessenger.Publish(new OpenContextMenuRequest
			{
				Sender = this,
				ContextMenuUser = this,
				ScreenPosition = Input.mousePosition
			});
		}
	}
}