using Stats;
using UnityEngine;

namespace GameWorld.Characters.Enemies
{
	[CreateAssetMenu(menuName = "Stats/Enemy", fileName = "EnemyStats", order = 0)]
	public class EnemyStats : ScriptableObject
	{
		[SerializeField] private MovementStats movementStats;

		public MovementStats MovementStats => movementStats;
	}
}