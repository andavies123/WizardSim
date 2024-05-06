﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.ContextMenus
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Interactable))]
	public class ContextMenuUser : MonoBehaviour
	{
		private readonly List<ContextMenuItem> _menuItems = new();

		public static event EventHandler<ContextMenuUserEventArgs> RequestMenuOpen;
		public event EventHandler MenuClosed;

		public Interactable Interactable { get; private set; }
		public IReadOnlyList<ContextMenuItem> AllMenuItems => _menuItems;

		public void AddMenuItem(ContextMenuItem menuItem) => _menuItems.Add(menuItem);
		public void UpdateMenuItems() => _menuItems.ForEach(item => item.RecalculateVisibility());
		
		public void OpenMenu() => RequestMenuOpen?.Invoke(this, new ContextMenuUserEventArgs(this));
		public void CloseMenu() => MenuClosed?.Invoke(this, EventArgs.Empty);

		private void Awake() => Interactable = GetComponent<Interactable>();
	}
}