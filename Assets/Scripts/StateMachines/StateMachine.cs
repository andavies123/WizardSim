using System;
using System.Collections.Generic;

namespace StateMachines
{
	public class StateMachine
	{
		private readonly HashSet<IState> _states = new();
		private readonly Dictionary<(IState, string), (IState, Action)> _stateTransitionTable = new();
		
		public string CurrentStateDisplayName => CurrentState?.DisplayName ?? "N/A";
		public string CurrentStateDisplayStatus => CurrentState?.DisplayStatus ?? "N/A";
	
		public IState DefaultState { get; private set; }
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
		
		public void AddStateTransition(IState finishedState, string exitReason, IState nextState, Action initCallback)
		{
			if (finishedState != null && _states.Add(finishedState))
				finishedState.ExitRequested += OnStateExitRequested;
			if (nextState != null && _states.Add(nextState))
				nextState.ExitRequested += OnStateExitRequested;
			
			_stateTransitionTable.Add((finishedState, exitReason), (nextState, initCallback));
		}

		private void OnStateExitRequested(object sender, string exitReason)
		{
			if (sender is not IState finishedState)
				return;

			if (_stateTransitionTable.TryGetValue((finishedState, exitReason), out (IState nextState, Action initCallback) args))
			{
				args.initCallback?.Invoke();
				SetCurrentState(args.nextState);
			}
			else
			{
				SetCurrentState(DefaultState);
			}
		}
	}
}