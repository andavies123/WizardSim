using Stats;
using UnityEngine;

namespace Wizards
{
	[CreateAssetMenu(menuName = "Stats/Wizard", fileName = "WizardStats", order = 0)]
	public class WizardStats : ScriptableObject
	{
		[SerializeField] private MovementStats movementStats;

		public MovementStats MovementStats => movementStats;
	}
}