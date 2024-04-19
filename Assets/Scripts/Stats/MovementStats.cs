using UnityEngine;

namespace Stats
{
	[CreateAssetMenu(menuName = "Stats/General/Movement", fileName = "MovementStats", order = 0)]
	public class MovementStats : ScriptableObject
	{
		[SerializeField] private float speed = 2;
		[SerializeField] private float rotationSpeed = 1;

		public float Speed => speed;
		public float RotationSpeed => rotationSpeed;
	}
}