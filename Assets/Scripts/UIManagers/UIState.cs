﻿using UnityEngine;

namespace UIManagers
{
	[RequireComponent(typeof(Canvas))]
	public abstract class UIState : MonoBehaviour
	{
		private Canvas _canvas;

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
			
			if (!_canvas)
				Debug.LogWarning("No canvas found on this UIManager");
		}

		protected abstract void OnStateEnabled();
		protected abstract void OnStateDisabled();
	}
}