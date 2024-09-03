namespace StateMachines
{
	public class StateMachine
	{
		public string CurrentStateDisplayName => CurrentState?.DisplayName ?? "N/A";
		public string CurrentStateDisplayStatus => CurrentState?.DisplayStatus ?? "N/A";
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
	}
}