using UnityEngine;

namespace Wizards
{
	[CreateAssetMenu(menuName = "Create WizardStats", fileName = "WizardStats", order = 0)]
	public class WizardStats : ScriptableObject
	{
		[SerializeField] private float movementSpeed = 2;

		public float MovementSpeed => movementSpeed;
	}
}