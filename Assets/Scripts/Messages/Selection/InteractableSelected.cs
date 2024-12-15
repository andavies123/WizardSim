using System.Text;
using MessagingSystem;
using UI;

namespace Messages.Selection
{
	/// <summary>
	/// Raised everytime an interactable gets selected
	/// This doesn't show the current selection in the system, just the latest interactable that was selected 
	/// </summary>
	public class InteractableSelected : Message
	{
		public Interactable SelectedInteractable { get; set; }
		public InteractionType InteractionType { get; set; }

		public override StringBuilder GetDisplayText()
		{
			string selectedInteractableName = SelectedInteractable ? SelectedInteractable.name : "(None)";
			
			StringBuilder stringBuilder = base.GetDisplayText();
			stringBuilder.AppendLine($"Interaction Type: {InteractionType}");
			stringBuilder.AppendLine($"Selected Interactable: {selectedInteractableName}");
			return stringBuilder;
		}
	}
}