using GameWorld.Characters.Wizards;
using System;
using System.Collections.Generic;

namespace GameWorld.Settlements
{
	public interface IWizardRepo
	{
		/// <summary>
		/// Raised when a wizard is added to the repository
		/// </summary>
		event EventHandler<Wizard> WizardAdded;

		/// <summary>
		/// Raised when a wizard is removed from the repository
		/// </summary>
		event EventHandler<Wizard> WizardRemoved;

		/// <summary>
		/// Dictionary of all wizards (Key => Wizard ID, Value => Wizard)
		/// </summary>
		IReadOnlyDictionary<Guid, Wizard> AllWizards { get; }

		/// <summary>
		/// Attempts to add a wizard to the repository using the wizard Id
		/// </summary>
		/// <param name="wizard">The wizard object to add to the repository</param>
		/// <returns>True if the wizard was successfully added. False if the wizard was not added or already exists</returns>
		bool TryAddWizard(Wizard wizard);

		/// <summary>
		/// Attempts to remove a wizard from the repository
		/// </summary>
		/// <param name="wizardId">The unique wizard Id</param>
		/// <returns>True if the wizard was successfully removed. False if the wizard was not removed or does not exist</returns>
		bool TryRemoveWizard(Guid wizardId);
	}
}