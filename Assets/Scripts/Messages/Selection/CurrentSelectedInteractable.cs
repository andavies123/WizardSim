﻿using MessagingSystem;
using System.Text;
using UI;

namespace Messages.Selection
{
	public class CurrentSelectedInteractable : Message
	{
		public Interactable SelectedInteractable { get; set; }
		public InteractionType InteractionType { get; set; }

		public override StringBuilder GetDisplayText()
		{
			StringBuilder stringBuilder = base.GetDisplayText();
			stringBuilder.AppendLine($"Interaction Type: {InteractionType}");
			stringBuilder.AppendLine($"Selected Interactable: {SelectedInteractable.name}");
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