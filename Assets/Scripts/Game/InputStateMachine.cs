using InputStates;

namespace Game
{
	public class InputStateMachine
	{
		private IInputState currentInputState;

		public void SetCurrentState(IInputState inputState)
		{
			currentInputState?.Disable();
			currentInputState = inputState;
			currentInputState?.Enable();
		}
	}
}