using Game.Common;
using UI;

namespace Game.Events
{
	public class InteractionEvents
	{
		public GameEvent<SelectedInteractableEventArgs> InteractableSelected { get; } = new();
	}

	public class SelectedInteractableEventArgs : GameEventArgs
	{
		public SelectedInteractableEventArgs(Interactable selectedInteractable, SelectionType selectionType)
		{
			SelectedInteractable = selectedInteractable;
			SelectionType = selectionType;
		}
		
		public Interactable SelectedInteractable { get; }
		public SelectionType SelectionType { get; }
	}
}