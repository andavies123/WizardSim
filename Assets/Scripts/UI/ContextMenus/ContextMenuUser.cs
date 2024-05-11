using System;
using System.Collections.Generic;
using Game.MessengerSystem;
using UI.Messages;
using UnityEngine;

namespace UI.ContextMenus
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Interactable))]
	public class ContextMenuUser : MonoBehaviour
	{
		private readonly List<ContextMenuItem> _menuItems = new();
		private bool _isContextMenuOpen = false;
		
		public event EventHandler IsContextMenuOpenValueChanged;
		
		public Interactable Interactable { get; private set; }
		public IReadOnlyList<ContextMenuItem> AllMenuItems => _menuItems;

		public bool IsContextMenuOpen
		{
			get => _isContextMenuOpen;
			set
			{
				if (_isContextMenuOpen == value)
					return;

				_isContextMenuOpen = value;
				IsContextMenuOpenValueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void AddMenuItem(ContextMenuItem menuItem) => _menuItems.Add(menuItem);
		public void UpdateMenuItems() => _menuItems.ForEach(item => item.RecalculateVisibility());
		
		protected virtual void Awake()
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

		private void OnInteractableContextMenuOpened(object sender, EventArgs args) =>
			GlobalMessenger.Publish(new OpenContextMenuRequest(this));
	}
}