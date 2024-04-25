using InputStates;

namespace Game
{
	public class InputStateMachine
	{
		private IInput currentInput;

		public void SetCurrentState(IInput input)
		{
			currentInput?.Disable();
			currentInput = input;
			currentInput?.Enable();
		}
	}
}