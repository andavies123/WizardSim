using UI;

namespace Game.Events
{
	public class InteractionEvents
	{
		public GameEvent<SelectedInteractableEventArgs> InteractableSelected { get; } = new();
		public GameEvent<SelectedInteractableEventArgs> CurrentSelectedInteractableUpdated { get; } = new();
	}

	public class SelectedInteractableEventArgs : GameEventArgs
	{
		public Interactable SelectedInteractable { get; set; }
		public SelectionType SelectionType { get; set; }
	}

	public enum SelectionType
	{
		PrimarySelection,
		SecondarySelection
	}
}