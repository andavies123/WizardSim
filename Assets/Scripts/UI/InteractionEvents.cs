using System;
using UnityEngine;

namespace UI
{
	[CreateAssetMenu(menuName = "Interaction Events", fileName = "InteractionEvents", order = 0)]
	public class InteractionEvents : ScriptableObject
	{
		public event Action<Action<MonoBehaviour>> InteractionRequested;
		public event Action InteractionCanceled;

		public void RequestInteraction(Action<MonoBehaviour> onInteraction)
		{
			InteractionRequested?.Invoke(onInteraction);
		}

		public void CancelInteraction()
		{
			InteractionCanceled?.Invoke();
		}
	}
}