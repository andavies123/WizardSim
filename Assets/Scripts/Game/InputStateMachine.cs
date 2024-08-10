using InputStates;

namespace Game
{
	public class InputStateMachine
	{
		public IInputState CurrentInputState { get; private set; }

		public void SetCurrentState(IInputState inputState)
		{
			CurrentInputState?.Disable();
			CurrentInputState = inputState;
			CurrentInputState?.Enable();
		}
	}
}