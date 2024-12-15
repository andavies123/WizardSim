using System;
using Messages.Selection;
using MessagingSystem;
using UI;
using UnityEngine;

namespace Game
{
	public class SelectionManager : MonoBehaviour
	{
		private MessageBroker _messageBroker;

		private Interactable _currentSelectedPrimary;
		private Interactable _currentSelectedSecondary;

		private void Awake()
		{
			_messageBroker = Dependencies.Get<MessageBroker>();

			_messageBroker.Subscribe(Subscription.CreateSubscription<InteractableSelected>(
				this, OnInteractableSelectedReceived));
		}

		private void OnInteractableSelectedReceived(IMessage message)
		{
			if (message is InteractableSelected selectedMessage)
			{
				switch (selectedMessage.InteractionType)
				{
					case InteractionType.PrimarySelection:
						HandleNewPrimarySelection(selectedMessage.SelectedInteractable);
						break;
					case InteractionType.SecondarySelection:
						HandleNewSecondarySelection(selectedMessage.SelectedInteractable);
						break;
					default:
						throw new IndexOutOfRangeException(selectedMessage.InteractionType.ToString());
				}
			}
		}

		private void HandleNewPrimarySelection(Interactable newSelection)
		{
			_currentSelectedPrimary = _currentSelectedPrimary == newSelection ? null : newSelection;
			SendSelectedMessage(InteractionType.PrimarySelection, _currentSelectedPrimary);
		}

		private void HandleNewSecondarySelection(Interactable newSelection)
		{
			_currentSelectedSecondary = newSelection;
			SendSelectedMessage(InteractionType.SecondarySelection, _currentSelectedSecondary);
		}

		private void SendSelectedMessage(InteractionType interactionType, Interactable selected)
		{
			_messageBroker.PublishPersistant(
				new CurrentSelectedInteractableKey
				{
					InteractionType = interactionType,
					Sender = this,
				},
				new CurrentSelectedInteractable
				{
					InteractionType = interactionType,
					SelectedInteractable = selected,
					Sender = this
				});
		}
	}
}