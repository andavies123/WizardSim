﻿using MessagingSystem;
using System.Text;
using UI;

namespace Messages.Selection
{
	/// <summary>
	/// Contains the current selected interactable in the system
	/// This message only gets raised when the systems current selection changes
	/// </summary>
	public class CurrentSelectedInteractable : Message
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

	public class CurrentSelectedInteractableKey : IMessageKey
	{
		public InteractionType InteractionType { get; set; }
		public string CompareString => InteractionType.ToString();
		public object Sender { get; set; }
	}

	public enum InteractionType
	{
		PrimarySelection,
		SecondarySelection
	}
}