using UI;

namespace InputStates.InputEventArgs
{
	public class OpenInfoWindowEventArgs
	{
		public OpenInfoWindowEventArgs(Interactable interactable)
		{
			Interactable = interactable;
		}
        
		public Interactable Interactable { get; }
	}
}