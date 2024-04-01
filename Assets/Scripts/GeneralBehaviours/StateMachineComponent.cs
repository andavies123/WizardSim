using StateMachines;
using UnityEngine;

namespace GeneralBehaviours
{
	public abstract class StateMachineComponent<T> : StateMachineComponent where T : IState
	{
		protected readonly StateMachine<T> StateMachine = new();
		
		public override string CurrentStateDisplayName => StateMachine.CurrentStateDisplayName;
		public override string CurrentStateDisplayStatus => StateMachine.CurrentStateDisplayStatus;
	}

	public abstract class StateMachineComponent : MonoBehaviour
	{
		public abstract string CurrentStateDisplayName { get; }
		public abstract string CurrentStateDisplayStatus { get; }
	}
}