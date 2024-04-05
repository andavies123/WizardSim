using System;
using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(MouseInteractionEvents))]
	public class Interactable : MonoBehaviour
	{
		public static event Action<MonoBehaviour> LeftMousePressed;

		[SerializeField] private MonoBehaviour mainComponent;
		
		private MouseInteractionEvents _mouseInteractionEvents;

		private void Awake()
		{
			_mouseInteractionEvents = GetComponent<MouseInteractionEvents>();

			_mouseInteractionEvents.LeftMousePressed += OnLeftMousePressed;
		}

		private void OnDestroy()
		{
			_mouseInteractionEvents.LeftMousePressed -= OnLeftMousePressed;
		}

		private void OnLeftMousePressed() => LeftMousePressed?.Invoke(mainComponent);
	}
}