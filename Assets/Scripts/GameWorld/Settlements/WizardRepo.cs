using GameWorld.Characters.Wizards;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GameWorld.Characters;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Settlements
{
	public class WizardRepo : MonoBehaviour
	{
		[SerializeField, Required] private Transform wizardContainer;
		
		private readonly ConcurrentDictionary<Guid, Wizard> _wizards = new();

		public event Action<Wizard> WizardAdded;
		public event Action<Wizard> WizardRemoved;

		public IReadOnlyDictionary<Guid, Wizard> AllWizards => _wizards;

		public bool TryAddWizard(Wizard wizard)
		{
			if (!wizard || wizard.Id == Guid.Empty)
				return false;

			if (!_wizards.TryAdd(wizard.Id, wizard))
				return false;

			wizard.Death.Died += OnWizardDied;
			WizardAdded?.Invoke(wizard);
			return true;
		}

		public bool TryRemoveWizard(Guid wizardId)
		{
			if (wizardId == Guid.Empty)
				return false;

			if (!_wizards.TryRemove(wizardId, out Wizard wizard))
				return false;

			wizard.Transform.parent = null; // We don't want to keep the wizard in the container
			wizard.Death.Died -= OnWizardDied;
			WizardRemoved?.Invoke(wizard);
			return true;
		}

		public bool TryGetWizardById(Guid wizardId, out Wizard wizard)
		{
			wizard = null;
			
			return wizardId != Guid.Empty && _wizards.TryGetValue(wizardId, out wizard);
		}

		public IList<Wizard> GetAllWizardsByType(WizardType wizardType)
		{
			return _wizards.Values.Where(wizard => wizard.WizardType == wizardType).ToList();
		}

		private void OnWizardDied(object sender, CharacterDiedEventArgs args)
		{
			if (args.DeadCharacter is Wizard deadWizard)
				TryRemoveWizard(deadWizard.Id);
		}
	}
}
