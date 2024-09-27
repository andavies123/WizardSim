using System;
using System.Collections.Generic;
using GameWorld.Characters.Wizards;

namespace GameWorld.Settlements.Interfaces
{
	public interface IWizardRepo
	{
		/// <summary>
		/// Raised when a wizard is added to the repository
		/// </summary>
		event Action<Wizard> WizardAdded;

		/// <summary>
		/// Raised when a wizard is removed from the repository
		/// </summary>
		event Action<Wizard> WizardRemoved;

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

		/// <summary>
		/// Attempts to get a wizard using the unique Wizard ID
		/// </summary>
		/// <param name="wizardId">The unique ID of the wizard to look for</param>
		/// <param name="wizard">The wizard object that was found with the given ID. Null if none were found</param>
		/// <returns>True if a wizard was found with the given ID. False if no wizards were found</returns>
		bool TryGetWizardById(Guid wizardId, out Wizard wizard);
		
		/// <summary>
		/// Gets all wizards of a certain <see cref="WizardType"/>
		/// </summary>
		/// <param name="wizardType">The type of wizard to query for</param>
		/// <returns>Collection of all wizards of the given <see cref="WizardType"/></returns>
		IList<Wizard> GetAllWizardsByType(WizardType wizardType);
	}
}