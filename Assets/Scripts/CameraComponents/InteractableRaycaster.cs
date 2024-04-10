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
        
		public event Action<Interactable> InteractableSelectedPrimary;
        public event Action<Interactable> InteractableSelectedSecondary;
        public event Action<Interactable> InteractableHoverBegin;
        public event Action<Interactable> InteractableHoverEnd;

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
			{
				interactable.PrimaryActionSelect();
				InteractableSelectedPrimary?.Invoke(interactable);
			}

			if (Input.GetMouseButtonDown(InputUtilities.RightMouseButton))
			{
				interactable.SecondaryActionSelect();
				InteractableSelectedSecondary?.Invoke(interactable);
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