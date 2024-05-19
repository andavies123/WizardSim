using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wizards
{
	public class WizardManager : MonoBehaviour
	{
		private readonly Dictionary<Guid, Wizard> _wizards = new();

		public IReadOnlyDictionary<Guid, Wizard> Wizards => _wizards;

		public void Add(Wizard wizard) => _wizards.Add(wizard.Id, wizard);
		public void Remove(Wizard wizard) => _wizards.Remove(wizard.Id);
	}
}