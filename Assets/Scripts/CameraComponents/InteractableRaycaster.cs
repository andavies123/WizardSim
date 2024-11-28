﻿using Game;
using Messages.Selection;
using MessagingSystem;
using System;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace CameraComponents
{
	public class   InteractableRaycaster : MonoBehaviour
	{
		[SerializeField] private new Camera camera;
		[SerializeField] private float maxRaycastDistance = 1000;

		private MessageBroker _messageBroker;
		private Interactable _currentHover;
		private Interactable _currentSelected;
		private bool _isPointerOverUI = false;
        
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
				return; // We don't want to continue if we are over UI
			
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
			{
				BeginNewSelection(interactable);
			}

			if (Input.GetMouseButtonDown(InputUtilities.RightMouseButton))
			{
				interactable.SelectSecondaryAction();
				InteractableSelectedSecondary?.Invoke(this, new InteractableRaycasterEventArgs(interactable));

				_messageBroker.PublishPersistant(
					new CurrentSelectedInteractableKey
					{
						InteractionType = InteractionType.SecondarySelection,
						Sender = this,
					},
					new CurrentSelectedInteractable
					{
						InteractionType = InteractionType.SecondarySelection,
						SelectedInteractable = interactable,
						Sender = this
					});
			}
		}

		private void HandleNoInteractableFound()
		{
			if (_currentHover)
				EndCurrentHover();
			else
				NonInteractableHoverBegin?.Invoke(this, EventArgs.Empty);
			
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

				_messageBroker.PublishPersistant(
					new CurrentSelectedInteractableKey
					{
						InteractionType = InteractionType.PrimarySelection,
						Sender = this,
					},
					new CurrentSelectedInteractable
					{
						InteractionType = InteractionType.PrimarySelection,
						SelectedInteractable = _currentSelected,
						Sender = this
					});
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