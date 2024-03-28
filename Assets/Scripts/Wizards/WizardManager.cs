using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wizards
{
	public class WizardManager : MonoBehaviour
	{
		private readonly Dictionary<Guid, Wizard> _wizards = new();

		public IReadOnlyDictionary<Guid, Wizard> Wizards => _wizards;

		public void AddWizard(Wizard wizard)
		{
			_wizards.TryAdd(wizard.Id, wizard);
		}

		public void RemoveWizard(Wizard wizard)
		{
			_wizards.Remove(wizard.Id);
		}
	}
}