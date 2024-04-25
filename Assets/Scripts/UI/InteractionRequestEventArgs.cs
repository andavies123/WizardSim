using System;
using UnityEngine;

namespace UI
{
	public class InteractionRequestEventArgs : EventArgs
	{
		public InteractionRequestEventArgs(Action<MonoBehaviour> onInteractionCallback)
		{
			OnInteractionCallback = onInteractionCallback;
		}
		
		public Action<MonoBehaviour> OnInteractionCallback { get; }
	}
}