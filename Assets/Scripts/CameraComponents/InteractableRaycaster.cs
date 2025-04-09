using System;
using Extensions;
using Game.Common;
using Game.Events;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace CameraComponents
{
	public class InteractableRaycaster : MonoBehaviour
	{
		[SerializeField] private new Camera camera;
		[SerializeField] private float maxRaycastDistance = 1000;

		private Interactable _currentHover;
		private Interactable _currentSelected;
		private bool _isPointerOverUI;
        
		public event EventHandler<InteractableRaycasterEventArgs> InteractableSelectedPrimary;
        public event EventHandler<InteractableRaycasterEventArgs> InteractableSelectedSecondary;
        public event EventHandler<InteractableRaycasterEventArgs> InteractableHoverBegin;
        public event EventHandler<InteractableRaycasterEventArgs> InteractableHoverEnd;
        public event EventHandler NonInteractableSelectedPrimary;
        public event EventHandler NonInteractableSelectedSecondary;
        public event EventHandler NonInteractableHoverBegin;
        public event EventHandler UIHoverBegin;
        public event EventHandler UIHoverEnd;

        public bool IsInteractableCurrentlyHovered => _currentHover;
        
		private void Update()
		{
			bool isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();

			if (isPointerOverGameObject != _isPointerOverUI)
			{
				_isPointerOverUI = isPointerOverGameObject;

				if (_isPointerOverUI)
					UIHoverBegin?.Invoke(this, EventArgs.Empty);
				else
					UIHoverEnd?.Invoke(this, EventArgs.Empty);
			}

			if (_isPointerOverUI)
			{
				EndCurrentHover();
				return; // We don't want to continue if we are over UI
			}
			
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			Interactable interactable = null;

			bool didRaycastHit = Physics.Raycast(ray, out RaycastHit hitInfo, maxRaycastDistance);

			bool interactableFound = hitInfo.transform && 
			                         (hitInfo.transform.TryGetComponent(out interactable) || 
			                          hitInfo.transform.TryGetComponentInParent(out interactable));
			
			if (!didRaycastHit || !interactableFound)
				HandleNoInteractableFound();
			else
				HandleInteractableFound(interactable, hitInfo);
		}

		private void HandleInteractableFound(Interactable interactable, RaycastHit hitInfo)
		{
			BeginNewHover(interactable);
			_currentHover.LatestHoverRaycastHit = hitInfo;

			if (Input.GetMouseButtonDown(InputUtilities.LeftMouseButton))
			{
				BeginNewSelection(interactable, hitInfo);
			}

			if (Input.GetMouseButtonDown(InputUtilities.RightMouseButton))
			{
				interactable.SelectSecondaryAction();
				InteractableSelectedSecondary?.Invoke(this, new InteractableRaycasterEventArgs(interactable));

				RaiseInteractableSelected(SelectionType.SecondarySelection, interactable);
			}
		}

		private void HandleNoInteractableFound()
		{
			if (_currentHover)
				EndCurrentHover();
			else
				NonInteractableHoverBegin?.Invoke(this, EventArgs.Empty);

			if (Input.GetMouseButtonDown(InputUtilities.LeftMouseButton))
			{
				NonInteractableSelectedPrimary?.Invoke(this, EventArgs.Empty);
				RaiseInteractableSelected(SelectionType.PrimarySelection, null);
			}

			if (Input.GetMouseButtonDown(InputUtilities.RightMouseButton))
			{
				NonInteractableSelectedSecondary?.Invoke(this, EventArgs.Empty);
				RaiseInteractableSelected(SelectionType.SecondarySelection, null);
			}
		}

		private void BeginNewSelection(Interactable newSelection, RaycastHit hitInfo)
		{
			if (_currentSelected)
			{
				_currentSelected.IsSelected = false;
				_currentSelected.LatestSelectedRaycastHit = null;
			}

			_currentSelected = newSelection;

			if (_currentSelected)
			{
				_currentSelected.IsSelected = true;
				_currentSelected.LatestSelectedRaycastHit = hitInfo;
				_currentSelected.SelectPrimaryAction();
				InteractableSelectedPrimary?.Invoke(this, new InteractableRaycasterEventArgs(_currentSelected));

				RaiseInteractableSelected(SelectionType.PrimarySelection, _currentSelected);
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

			_currentHover.LatestHoverRaycastHit = null;
			_currentHover.IsHovered = false;
			InteractableHoverEnd?.Invoke(this, new InteractableRaycasterEventArgs(_currentHover));
			_currentHover = null;
		}

		private void RaiseInteractableSelected(SelectionType selectionType, Interactable selectedInteractable)
		{
			GameEvents.Interaction.InteractableSelected.Raise(this,
				new SelectedInteractableEventArgs(selectedInteractable, selectionType));
		}
	}
}