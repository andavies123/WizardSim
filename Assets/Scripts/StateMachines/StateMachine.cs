using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachines
{
	public class StateMachine
	{
		private readonly Dictionary<StateTransitionFrom, List<StateTransitionTo>> _stateTransitions = new();
		
		public string CurrentStateDisplayName => CurrentState?.DisplayName ?? "N/A";
		public string CurrentStateDisplayStatus => CurrentState?.DisplayStatus ?? "N/A";
	
		public IState DefaultState { get; set; }
		public IState CurrentState { get; private set; }

		public bool IsCurrentState(IState state) => state == CurrentState;
		
		public void SetCurrentState(IState newState)
		{
			if (CurrentState != null)
			{
				CurrentState.ExitRequested -= OnStateExitRequested;
				CurrentState.End();
			}

			CurrentState = newState;

			if (CurrentState != null)
			{
				CurrentState.ExitRequested += OnStateExitRequested;
				CurrentState.Begin();
			}
		}

		public void Update()
		{
			CurrentState?.Update();
		}
		
		public void AddStateTransition(StateTransitionFrom transitionFrom, params StateTransitionTo[] transitionTos)
		{
			if (!_stateTransitions.TryGetValue(transitionFrom, out _))
			{
				_stateTransitions.Add(transitionFrom, transitionTos.ToList());
			}
		}

		public void ClearStateTransitions() => _stateTransitions.Clear();

		private void OnStateExitRequested(object sender, string exitReason)
		{
			if (sender is not IState finishedState)
				return;

			if (_stateTransitions.TryGetValue(new StateTransitionFrom(finishedState, exitReason), out List<StateTransitionTo> transitions))
			{
				// Loop through all transitions until we can transition to a new state
				foreach (StateTransitionTo transition in transitions)
				{
					if (transition.TransitionValidation.Invoke())
					{
						transition.NextStateInit?.Invoke();
						SetCurrentState(transition.NextState);
						return;
					}
				}
			}
			
			if (DefaultState == null)
			{
				Debug.LogWarning("Attempting to change state to the default state but the default state does not exist...");
				return;
			}
				
			SetCurrentState(DefaultState);
		}
	}

	public class StateTransitionFrom
	{
		public IState FinishedState { get; }
		public string ExitReason { get; }

		public StateTransitionFrom(IState finishedState, string exitReason)
		{
			FinishedState = finishedState;
			ExitReason = exitReason;
		}

		public override bool Equals(object obj)
		{
			if (obj is not StateTransitionFrom other)
				return false;

			return Equals(FinishedState, other.FinishedState) && string.Equals(ExitReason, other.ExitReason);
		}

		public override int GetHashCode() => HashCode.Combine(FinishedState, ExitReason);
	}

	public class StateTransitionTo
	{
		public IState NextState { get; }
		public Action NextStateInit { get; }
		public Func<bool> TransitionValidation { get; }

		public StateTransitionTo(IState nextState, Action nextStateInit, Func<bool> transitionValidation)
		{
			NextState = nextState;
			NextStateInit = nextStateInit;
			TransitionValidation = transitionValidation ?? (() => true);
		}
	}
}