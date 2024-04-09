using System;
using UnityEngine;

namespace UI
{
	public class Interactable : MonoBehaviour
	{
		private bool _isHovered = false;
		private bool _isSelectedPrimary = false;
		private bool _isSelectedSecondary = false;

		public event Action<bool> IsHoveredValueChanged;
		public event Action<bool> IsSelectedPrimaryValueChanged;
		public event Action<bool> IsSelectedSecondaryValueChanged;
		
		public bool IsHovered
		{
			get => _isHovered;
			set
			{
				if (_isHovered == value)
					return;
				
				_isHovered = value;
				IsHoveredValueChanged?.Invoke(_isHovered);
			}
		}

		public bool IsSelectedPrimary
		{
			get => _isSelectedPrimary;
			set
			{
				if (_isSelectedPrimary == value)
					return;

				_isSelectedPrimary = value;
				IsSelectedPrimaryValueChanged?.Invoke(_isSelectedPrimary);
			}
		}

		public bool IsSelectedSecondary
		{
			get => _isSelectedSecondary;
			set
			{
				if (_isSelectedSecondary == value)
					return;

				_isSelectedSecondary = value;
				IsSelectedSecondaryValueChanged?.Invoke(_isSelectedSecondary);
			}
		}
	}
}