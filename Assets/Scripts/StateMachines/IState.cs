namespace StateMachines
{
	public interface IState
	{
		string DisplayName { get; }
		string DisplayStatus { get; }
		
		/// <summary>
		/// Called once when the state is first set to the current state
		/// </summary>
		void Begin();
		
		/// <summary>
		/// Called once per frame. Contains logic
		/// </summary>
		void Update();
		
		/// <summary>
		/// Called once when this state won't be the current state anymore
		/// </summary>
		void End();
	}
}