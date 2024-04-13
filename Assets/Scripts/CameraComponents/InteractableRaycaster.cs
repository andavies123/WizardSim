using System;
using UI;
using UnityEngine;
using Utilities;

namespace CameraComponents
{
	public class InteractableRaycaster : MonoBehaviour
	{
		[SerializeField] private new Camera camera;
		[SerializeField] private float maxRaycastDistance = 1000;

		private Interactable _currentHover;
		private Interactable _currentSelected;
        
		public event Action<Interactable> InteractableSelectedPrimary;
        public event Action<Interactable> InteractableSelectedSecondary;
        public event Action<Interactable> InteractableHoverBegin;
        public event Action<Interactable> InteractableHoverEnd;

        public bool IsInteractableCurrentlyHovered => _currentHover;

		private void Update()
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			if (!Physics.Raycast(ray, out RaycastHit hitInfo, maxRaycastDistance) ||
			    !hitInfo.transform.TryGetComponent(out Interactable interactable))
			{
				EndCurrentHover();
				return;
			}
			
			BeginNewHover(interactable);

			if (Input.GetMouseButtonDown(InputUtilities.LeftMouseButton))
				BeginNewSelection(interactable);

			if (Input.GetMouseButtonDown(InputUtilities.RightMouseButton))
			{
				interactable.SelectSecondaryAction();
				InteractableSelectedSecondary?.Invoke(interactable);
			}
		}

		private void BeginNewSelection(Interactable newSelection)
		{
			if (_currentSelected)
				_currentSelected.IsSelected = false;

			_currentSelected = newSelection;

			if (_currentSelected)
			{
				_currentSelected.IsSelected = true;
				_currentSelected.SelectPrimaryAction();
				InteractableSelectedPrimary?.Invoke(_currentSelected);
			}
		}

		private void BeginNewHover(Interactable newHover)
		{
			if (!newHover)
				return;
			
			if (_currentHover && _currentHover == newHover)
				return;
			
			if (_currentHover && _currentHover != newHover)
				EndCurrentHover();

			_currentHover = newHover;
			_currentHover.IsHovered = true;
			InteractableHoverBegin?.Invoke(_currentHover);
		}

		private void EndCurrentHover()
		{
			if (!_currentHover)
				return;

			_currentHover.IsHovered = false;
			InteractableHoverEnd?.Invoke(_currentHover);
			_currentHover = null;
		}
	}
}