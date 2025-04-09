using System;
using Game.Common;
using Game.Events;
using UI;
using UnityEngine;

namespace Game
{
	public class SelectionManager : MonoBehaviour
	{
		private Interactable _currentSelectedPrimary;
		private Interactable _currentSelectedSecondary;

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
			_currentSelectedPrimary = _currentSelectedPrimary == newSelection ? null : newSelection;
			
			GameEvents.Interaction.PrimarySelectedInteractableUpdated.Raise(this,
				new SelectedInteractableEventArgs(_currentSelectedPrimary, SelectionType.PrimarySelection));
		}

		private void HandleNewSecondarySelection(Interactable newSelection)
		{
			_currentSelectedSecondary = newSelection;
			
			GameEvents.Interaction.SecondarySelectedInteractableUpdated.Raise(this,
				new SelectedInteractableEventArgs(_currentSelectedSecondary, SelectionType.SecondarySelection));
		}
	}
}