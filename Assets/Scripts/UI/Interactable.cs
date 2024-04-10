using System;
using UnityEngine;

namespace UI
{
	public class Interactable : MonoBehaviour
	{
		private bool _isHovered = false;

		public event Action<bool> IsHoveredValueChanged;

		public event Action PrimaryActionSelected;
		public event Action SecondaryActionSelected;
		
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

		public void PrimaryActionSelect() => PrimaryActionSelected?.Invoke();
		public void SecondaryActionSelect() => SecondaryActionSelected?.Invoke();
	}
}