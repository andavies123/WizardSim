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
        
		public event EventHandler<InteractableRaycasterEventArgs> InteractableSelectedPrimary;
        public event EventHandler<InteractableRaycasterEventArgs> InteractableSelectedSecondary;
        public event EventHandler<InteractableRaycasterEventArgs> InteractableHoverBegin;
        public event EventHandler<InteractableRaycasterEventArgs> InteractableHoverEnd;
        public event EventHandler NonInteractableSelectedPrimary;
        public event EventHandler NonInteractableSelectedSecondary;

        public bool IsInteractableCurrentlyHovered => _currentHover;

		private void Update()
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			Interactable interactable = null;

			bool didRaycastHit = Physics.Raycast(ray, out RaycastHit hitInfo, maxRaycastDistance);
			bool interactableFound = hitInfo.transform && hitInfo.transform.TryGetComponent(out interactable);

			if (!didRaycastHit || !interactableFound)
				HandleNoInteractableFound();
			else
				HandleInteractableFound(interactable);
		}

		private void HandleInteractableFound(Interactable interactable)
		{
			BeginNewHover(interactable);

			if (Input.GetMouseButtonDown(InputUtilities.LeftMouseButton))
				BeginNewSelection(interactable);

			if (Input.GetMouseButtonDown(InputUtilities.RightMouseButton))
			{
				interactable.SelectSecondaryAction();
				InteractableSelectedSecondary?.Invoke(this, new InteractableRaycasterEventArgs(interactable));
			}
		}

		private void HandleNoInteractableFound()
		{
			EndCurrentHover();
			
			if (Input.GetMouseButtonDown(InputUtilities.LeftMouseButton))
				NonInteractableSelectedPrimary?.Invoke(this, EventArgs.Empty);

			if (Input.GetMouseButtonDown(InputUtilities.RightMouseButton))
				NonInteractableSelectedSecondary?.Invoke(this, EventArgs.Empty);
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
				InteractableSelectedPrimary?.Invoke(this, new InteractableRaycasterEventArgs(_currentSelected));
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
			InteractableHoverBegin?.Invoke(this, new InteractableRaycasterEventArgs(_currentHover));
		}

		private void EndCurrentHover()
		{
			if (!_currentHover)
				return;

			_currentHover.IsHovered = false;
			InteractableHoverEnd?.Invoke(this, new InteractableRaycasterEventArgs(_currentHover));
			_currentHover = null;
		}
	}
}