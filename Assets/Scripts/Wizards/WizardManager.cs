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
			WizardAdded?.Invoke(this, new WizardManagerEventArgs(wizard));
		}

		public void Remove(Wizard wizard)
		{
			_wizards.Remove(wizard.Id);
			WizardRemoved?.Invoke(this, new WizardManagerEventArgs(wizard));
		}
	}

	public class WizardManagerEventArgs
	{
		public WizardManagerEventArgs(Wizard wizard)
		{
			Wizard = wizard;
		}
		
		public Wizard Wizard { get; }
	}
}