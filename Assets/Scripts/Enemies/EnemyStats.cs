using UnityEngine;

namespace Enemies
{
	[CreateAssetMenu(menuName = "Enemy/Stats", fileName = "EnemyStats", order = 0)]
	public class EnemyStats : ScriptableObject
	{
		[SerializeField] private float movementSpeed = 2;

		public float MovementSpeed => movementSpeed;
	}
}