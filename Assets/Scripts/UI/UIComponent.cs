using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(Canvas))]
	public abstract class UIComponent : MonoBehaviour
	{
		private Canvas _canvas;
		
		public void Show()
		{
			_canvas.enabled = true;
		}

		public void Hide()
		{
			_canvas.enabled = false;
		}

		protected virtual void Awake()
		{
			_canvas = GetComponent<Canvas>();
		}
		
		protected virtual void Start() { }
		protected virtual void OnDestroy() { }
	}
}