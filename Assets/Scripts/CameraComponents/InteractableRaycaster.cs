using System;
using Extensions;
using Game;
using Messages.Selection;
using MessagingSystem;
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

		private MessageBroker _messageBroker;
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

        private void Awake()
        {
	        _messageBroker = Dependencies.Get<MessageBroker>();
        }
        
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

				SendSelectedMessage(InteractionType.SecondarySelection, interactable);
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
				SendSelectedMessage(InteractionType.PrimarySelection, null);
			}

			if (Input.GetMouseButtonDown(InputUtilities.RightMouseButton))
			{
				NonInteractableSelectedSecondary?.Invoke(this, EventArgs.Empty);
				SendSelectedMessage(InteractionType.SecondarySelection, null);
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

				SendSelectedMessage(InteractionType.PrimarySelection, _currentSelected);
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

		private void SendSelectedMessage(InteractionType interactionType, Interactable selectedInteractable)
		{
			_messageBroker.PublishSingle(new InteractableSelected
			{
				InteractionType = interactionType,
				SelectedInteractable = selectedInteractable,
				Sender = this
			});
		}
	}
}