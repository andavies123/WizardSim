using GameWorld.Characters.Wizards;
using UnityEngine;

namespace GameWorld.Settlements.Interfaces
{
	public interface ISettlement
	{
		/// <summary>
		/// The name of the settlement given by the player
		/// </summary>
		string SettlementName { get; set; }

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
