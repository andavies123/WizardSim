namespace StateMachines
{
	public interface IState
	{
		string DisplayName { get; }
		string DisplayStatus { get; }
		
		void Begin();
		void Update();
		void End();
	}
}