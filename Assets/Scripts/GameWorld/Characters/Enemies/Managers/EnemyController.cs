using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Characters.Enemies.Managers
{
	public class EnemyController : MonoBehaviour
	{
		[SerializeField, Required] private EnemyRepo repo;
		[SerializeField, Required] private EnemyFactory factory;

		public void SpawnEnemy()
		{
			
		}
	}
}