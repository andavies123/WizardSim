using GameWorld.Characters.Wizards;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameWorld.Settlements
{
	public class WizardRepo : IWizardRepo
	{
		private readonly ConcurrentDictionary<Guid, Wizard> _wizards = new();

		public event EventHandler<Wizard> WizardAdded;
		public event EventHandler<Wizard> WizardRemoved;

		public IReadOnlyDictionary<Guid, Wizard> AllWizards => _wizards;

		public bool TryAddWizard(Wizard wizard)
		{
			if (!wizard || wizard.Id == Guid.Empty)
				return false;

			if (!_wizards.TryAdd(wizard.Id, wizard))
				return false;

			WizardAdded?.Invoke(this, wizard);
			return true;
		}

		public bool TryRemoveWizard(Guid wizardId)
		{
			if (wizardId == Guid.Empty)
				return false;

			if (!_wizards.TryRemove(wizardId, out Wizard wizard))
				return false;

			WizardRemoved?.Invoke(this, wizard);
			return true;
		}
	}
}
