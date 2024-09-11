using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachines
{
	public class StateMachine
	{
		private readonly HashSet<IState> _states = new();
		private readonly Dictionary<StateTransitionKey, List<StateTransitionValue>> _stateTransitions = new();
		
		public string CurrentStateDisplayName => CurrentState?.DisplayName ?? "N/A";
		public string CurrentStateDisplayStatus => CurrentState?.DisplayStatus ?? "N/A";
	
		public IState DefaultState { get; set; }
		public IState CurrentState { get; private set; }

		public bool IsCurrentState(IState state) => state == CurrentState;
		
		public void SetCurrentState(IState newState)
		{
			CurrentState?.End();
			CurrentState = newState;
			CurrentState?.Begin();
		}

		public void Update()
		{
			CurrentState?.Update();
		}
		
		public void AddStateTransition(StateTransitionKey transitionKey, params StateTransitionValue[] transitionValues)
		{
			if (transitionKey.FinishedState != null && _states.Add(transitionKey.FinishedState))
				transitionKey.FinishedState.ExitRequested += OnStateExitRequested;
			
			if (!_stateTransitions.TryGetValue(transitionKey, out List<StateTransitionValue> transitions))
			{
				transitions = new List<StateTransitionValue>();
				_stateTransitions.Add(transitionKey, transitions);
			}

			foreach (StateTransitionValue transition in transitionValues)
			{
				if (transition.NextState != null && _states.Add(transition.NextState))
					transition.NextState.ExitRequested += OnStateExitRequested;
				
				transitions.Add(transition);
			}
		}

		private void OnStateExitRequested(object sender, string exitReason)
		{
			if (sender is not IState finishedState)
				return;

			if (_stateTransitions.TryGetValue(new StateTransitionKey(finishedState, exitReason), out List<StateTransitionValue> transitions))
			{
				// Loop through all transitions until we can transition to a new state
				foreach (StateTransitionValue transition in transitions)
				{
					if (transition.TransitionValidation.Invoke())
					{
						transition.NextStateInit?.Invoke();
						SetCurrentState(transition.NextState);
						break;
					}
				}
			}
			else
			{
				if (DefaultState == null)
				{
					Debug.LogWarning("Attempting to change state to the default state but the default state does not exist...");
					return;
				}
				
				SetCurrentState(DefaultState);
			}
		}
	}

	public class StateTransitionKey
	{
		public IState FinishedState { get; }
		public string ExitReason { get; }

		public StateTransitionKey(IState finishedState, string exitReason)
		{
			FinishedState = finishedState;
			ExitReason = exitReason;
		}

		public override bool Equals(object obj)
		{
			if (obj is not StateTransitionKey other)
				return false;

			return Equals(FinishedState, other.FinishedState) && string.Equals(ExitReason, other.ExitReason);
		}

		public override int GetHashCode() => HashCode.Combine(FinishedState, ExitReason);
	}

	public class StateTransitionValue
	{
		public IState NextState { get; }
		public Action NextStateInit { get; }
		public Func<bool> TransitionValidation { get; }

		public StateTransitionValue(IState nextState, Action nextStateInit, Func<bool> transitionValidation)
		{
			NextState = nextState;
			NextStateInit = nextStateInit;
			TransitionValidation = transitionValidation ?? (() => true);
		}
	}
}