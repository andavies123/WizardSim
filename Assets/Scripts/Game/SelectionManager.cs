using System;
using Game.Common;
using Game.Events;
using UI;
using UnityEngine;

namespace Game
{
	public class SelectionManager : MonoBehaviour
	{
		public Interactable CurrentSelectedPrimary { get; private set; }
		public Interactable CurrentSelectedSecondary { get; private set; }

		public event EventHandler<SelectionUpdatedEventArgs> PrimarySelectionUpdated;
		public event EventHandler<SelectionUpdatedEventArgs> SecondarySelectionUpdated;

		private void Awake()
		{
			GameEvents.Interaction.InteractableSelected.Raised += OnInteractableSelectedReceived;
		}

		private void OnDestroy()
		{
			GameEvents.Interaction.InteractableSelected.Raised -= OnInteractableSelectedReceived;
		}

		private void OnInteractableSelectedReceived(object sender, SelectedInteractableEventArgs args)
		{
			switch (args.SelectionType)
			{
				case SelectionType.PrimarySelection:
					HandleNewPrimarySelection(args.SelectedInteractable);
					break;
				case SelectionType.SecondarySelection:
					HandleNewSecondarySelection(args.SelectedInteractable);
					break;
				default:
					throw new IndexOutOfRangeException(args.SelectionType.ToString());
			}
		}

		private void HandleNewPrimarySelection(Interactable newSelection)
		{
			CurrentSelectedPrimary = CurrentSelectedPrimary == newSelection ? null : newSelection;
			PrimarySelectionUpdated?.Invoke(this, new SelectionUpdatedEventArgs(CurrentSelectedPrimary));
		}

		private void HandleNewSecondarySelection(Interactable newSelection)
		{
			CurrentSelectedSecondary = newSelection;
			SecondarySelectionUpdated?.Invoke(this, new SelectionUpdatedEventArgs(CurrentSelectedSecondary));
		}
	}
	
	
	public class SelectionUpdatedEventArgs : EventArgs
	{
		public SelectionUpdatedEventArgs(Interactable selection) => Selection = selection;
		
		public Interactable Selection { get; }
	}
}