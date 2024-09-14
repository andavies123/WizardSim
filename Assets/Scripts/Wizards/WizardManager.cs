using GameWorld.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wizards
{
	public class WizardManager : MonoBehaviour
	{
		private readonly Dictionary<Guid, Wizard> _wizards = new();
		
		public event EventHandler<WizardManagerEventArgs> WizardAdded;
		public event EventHandler<WizardManagerEventArgs> WizardRemoved;
		
		public IReadOnlyDictionary<Guid, Wizard> Wizards => _wizards;

		public void Add(Wizard wizard)
		{
			_wizards.Add(wizard.Id, wizard);
			wizard.Death.Died += OnWizardDied;
			WizardAdded?.Invoke(this, new WizardManagerEventArgs(wizard));
		}

		public void Remove(Wizard wizard)
		{
			_wizards.Remove(wizard.Id);
			wizard.Death.Died -= OnWizardDied;
			WizardRemoved?.Invoke(this, new WizardManagerEventArgs(wizard));
		}

		public bool TryGetClosestWizard(Vector3 worldPosition, out Wizard closestWizard, out float distance)
		{
			closestWizard = null;
			distance = float.MaxValue;
            
			foreach (Wizard wizard in _wizards.Values)
			{
				float currentDistance = Vector3.Distance(worldPosition, wizard.Transform.position);

				if (currentDistance < distance)
				{
					distance = currentDistance;
					closestWizard = wizard;
				}
			}
			
			return (bool)closestWizard;
		}

		private void OnWizardDied(object sender, CharacterDiedEventArgs args)
		{
			if (args.DeadCharacter is Wizard deadWizard)
			{
				Remove(deadWizard);
			}
		}
	}
}