using GameWorld.Characters.Wizards;
using UnityEngine;

namespace GameWorld.Settlements.Interfaces
{
	public interface IWizardFactory
	{
		void Initialize(IWizardRepo wizardRepo);
		
		/// <summary>
		/// Creates a new wizard game object
		/// </summary>
		/// <param name="worldPosition">The position in the world that this wizard will be created at</param>
		/// <param name="wizardType">The type of wizard to create</param>
		void CreateWizard(Vector3 worldPosition, WizardType wizardType);
	}
}