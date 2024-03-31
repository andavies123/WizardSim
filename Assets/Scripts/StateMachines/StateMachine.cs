namespace StateMachines
{
	public class StateMachine<TStateBase> where TStateBase : IState
	{
		private IState _currentState;

		public string CurrentStateDisplayName => _currentState?.DisplayName ?? "N/A";
		public string CurrentStateDisplayStatus => _currentState?.DisplayStatus ?? "N/A";
		
		public void SetCurrentState(TStateBase newState)
		{
			_currentState?.End();
			_currentState = newState;
			_currentState?.Begin();
		}

		public void Update()
		{
			_currentState?.Update();
		}
	}
}