namespace InputStates
{
	public interface IInputState
	{
		bool ShowInteractions { get; }
		
		void Enable();
		void Disable();
	}
}