using System;
using UnityEngine;

namespace UI
{
	[CreateAssetMenu(menuName = "Interaction Events", fileName = "InteractionEvents", order = 0)]
	public class InteractionEvents : ScriptableObject
	{
		public event EventHandler<InteractionRequestEventArgs> InteractionRequested;
		public event EventHandler InteractionEnded;

		public void RequestInteraction(object sender, Action<MonoBehaviour> onInteraction)
		{
			InteractionRequested?.Invoke(sender, new InteractionRequestEventArgs(onInteraction));
		}

		public void EndInteraction(object sender)
		{
			InteractionEnded?.Invoke(sender, EventArgs.Empty);
		}
	}
}