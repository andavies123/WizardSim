using System;
using UnityEngine;

namespace Utilities
{
	[CreateAssetMenu(fileName = "GameEvent", menuName = "Actions/Game Event Vector 3", order = 1)]
	public class GameEventVector3 : ScriptableObject
	{
		public event Action<Vector3> Raised;

		public void RaiseEvent(Vector3 vector3) => Raised?.Invoke(vector3);
	}
}