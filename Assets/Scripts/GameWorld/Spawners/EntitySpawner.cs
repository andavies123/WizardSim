using UnityEngine;
using Utilities;

namespace GameWorld.Spawners
{
	public class EntitySpawner : MonoBehaviour
	{
		[SerializeField] private EntityManager entityManager;

		[Header("Prefabs")]
		[SerializeField] private GameObject entityPrefab;

		[Header("Spawn Settings")] 
		[SerializeField] private int initialSpawns = 5;
		[SerializeField] private Vector3Int spawnCenter = Vector3Int.zero;
		[SerializeField] private int minSpawnDistance = 10;
		[SerializeField] private int maxSpawnDistance = 25;

		public void SpawnEntity(Vector3 spawnPosition)
		{
			GameObject entityGameObject = Instantiate(entityPrefab, entityManager.transform);
			entityGameObject.transform.position = spawnPosition;
			Character character = entityGameObject.GetComponent<Character>();
			entityManager.Add(character);
		}
		
		private void Start()
		{
			LoopUtilities.Loop(initialSpawns, SpawnRandomEntity);
		}

		private void SpawnRandomEntity()
		{
			Vector2 randomDirection = Random.insideUnitCircle.normalized;
			float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
			Vector2 randomSpawn = randomDirection * randomDistance;
			
			SpawnEntity(new Vector3(
				(int)randomSpawn.x + spawnCenter.x + 0.5f,
				1,
				(int)randomSpawn.y + spawnCenter.z + 0.5f));
		}
	}
}