using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Characters.Wizards.Managers
{
	internal class WizardRepo : MonoBehaviour
	{
		[SerializeField, Required] private Transform wizardContainer;
		
		private readonly ConcurrentDictionary<Guid, Wizard> _wizards = new();
		
		public IReadOnlyDictionary<Guid, Wizard> AllWizards => _wizards;
		
		public bool TryAddWizard(Wizard wizard)
		{
			if (wizard && _wizards.TryAdd(wizard.Id, wizard))
			{
				wizard.transform.SetParent(wizardContainer);
				return true;
			}

			return false;
		}
		
		public bool TryRemoveWizard(Wizard wizard) => wizard && TryRemoveWizard(wizard.Id);
		public bool TryRemoveWizard(Guid wizardId)
		{
			if (_wizards.TryRemove(wizardId, out Wizard removedWizard))
			{
				removedWizard.transform.SetParent(null);
				return true;
			}

			return false;
		}
	}
}