using System;
using UnityEngine;

namespace Utilities
{
	[CreateAssetMenu(fileName = "GameEvent", menuName = "Actions/Game Event Vector 3", order = 1)]
	public class GameEventVector3 : ScriptableObject
	{
		public event EventHandler<Vector3> Raised;

		public void RaiseEvent(object sender, Vector3 vector3) => Raised?.Invoke(sender, vector3);
	}
}