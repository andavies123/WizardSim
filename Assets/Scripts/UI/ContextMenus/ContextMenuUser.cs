using System;
using UnityEngine;
using Utilities.Attributes;

namespace UI.ContextMenus
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Interactable))]
	public class ContextMenuUser : MonoBehaviour
	{
		[SerializeField, Required] private string objectType;
		
		private bool _isOpen = false;
		
		public event EventHandler IsContextMenuOpenValueChanged;

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

		private void Awake()
		{
			Interactable = GetComponent<Interactable>();
		}
	}
}