using UnityEngine;

namespace InputStates
{
	public abstract class InputState : ScriptableObject
	{
		public abstract void EnableInputs();
		public abstract void DisableInputs();
	}
}