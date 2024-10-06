using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GameStates
{
	[RequireComponent(typeof(Canvas))]
	public abstract class UIState : MonoBehaviour
	{
		private Canvas _canvas;

		public static bool IsMouseOverGameObject => EventSystem.current.IsPointerOverGameObject();

		public void Enable()
		{
			OnStateEnabled();
			_canvas.enabled = true;
		}

		public void Disable()
		{
			_canvas.enabled = false;
			OnStateDisabled();
		}

		protected virtual void Awake()
		{
			_canvas = GetComponent<Canvas>();
		}

		protected abstract void OnStateEnabled();
		protected abstract void OnStateDisabled();
	}
}