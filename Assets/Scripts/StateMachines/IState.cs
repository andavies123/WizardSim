using System;

namespace StateMachines
{
	public interface IState
	{
		/// <summary>
		/// Raised when a state is ready to exit
		/// Returns with the reason why an exit is requested
		/// </summary>
		event EventHandler<string> ExitRequested;
		
		/// <summary>
		/// The name of the state that should describe the entire state. This value should not change
		/// </summary>
		string DisplayName { get; }
		
		/// <summary>
		/// The current status of the state. This value should change based on what the state is doing
		/// </summary>
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