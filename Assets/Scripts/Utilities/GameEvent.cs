using System;
using UnityEngine;

namespace Utilities
{
	[CreateAssetMenu(fileName = "GameEvent", menuName = "Actions/Game Event", order = 0)]
	public class GameEvent : ScriptableObject
	{
		public event Action Raised;

		public void RaiseEvent() => Raised?.Invoke();
	}
}