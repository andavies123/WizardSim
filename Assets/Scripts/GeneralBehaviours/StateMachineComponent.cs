using StateMachines;
using UnityEngine;

namespace GeneralBehaviours
{
	public abstract class StateMachineComponent : MonoBehaviour
	{
		protected readonly StateMachine StateMachine = new();
        
		public virtual string CurrentStateDisplayName => StateMachine.CurrentStateDisplayName;
		public virtual string CurrentStateDisplayStatus => StateMachine.CurrentStateDisplayStatus;
		public IState CurrentState => StateMachine.CurrentState;
	}
}