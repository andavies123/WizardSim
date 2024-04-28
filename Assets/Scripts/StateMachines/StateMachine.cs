namespace StateMachines
{
	public class StateMachine<TStateBase> where TStateBase : IState
	{
		public string CurrentStateDisplayName => CurrentState?.DisplayName ?? "N/A";
		public string CurrentStateDisplayStatus => CurrentState?.DisplayStatus ?? "N/A";
		public IState CurrentState { get; private set; }

		public void SetCurrentState(TStateBase newState)
		{
			CurrentState?.End();
			CurrentState = newState;
			CurrentState?.Begin();
		}

		public void Update()
		{
			CurrentState?.Update();
		}
	}
}