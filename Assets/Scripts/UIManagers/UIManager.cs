using UnityEngine;

namespace UIManagers
{
	[RequireComponent(typeof(Canvas))]
	public abstract class UIManager : MonoBehaviour
	{
		private Canvas _canvas;

		public void Enable()
		{
			_canvas.enabled = true;
		}

		public void Disable()
		{
			_canvas.enabled = false;
		}

		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
		}
	}
}