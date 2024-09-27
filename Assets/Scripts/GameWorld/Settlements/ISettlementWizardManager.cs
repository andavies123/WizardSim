using GameWorld.Characters.Wizards;
using GameWorld.Settlements.Interfaces;
using UnityEngine;

namespace GameWorld.Settlements
{
	public interface ISettlementWizardManager : IInit, ICleanUp
	{
		/// <summary>
		/// Object that holds all references to wizards
		/// </summary>
		IWizardRepo Repo { get; }
		
		/// <summary>
		/// Object used to create wizard objects
		/// </summary>
		IWizardFactory Factory { get; }
		
		/// <summary>
		/// Object to manage the task system for wizards
		/// </summary>
		IWizardTaskManager TaskManager { get; }
		
		/// <summary>
		/// Attempts to get the closest wizard to a given position
		/// </summary>
		/// <param name="worldPosition">The position in the world</param>
		/// <param name="closestWizard">The wizard that is closest to <paramref name="worldPosition"/></param>
		/// <param name="distance">The distance the <paramref name="closestWizard"/> is to <paramref name="worldPosition"/></param>
		/// <returns>True if a wizard was found. False if there are no wizards at all</returns>
		bool TryGetClosestWizard(Vector3 worldPosition, out Wizard closestWizard, out float distance);
	}
}