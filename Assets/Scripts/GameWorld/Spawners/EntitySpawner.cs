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
		[SerializeField] private int spawnRadius = 10;

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
			Vector2 randomSpawn = Random.insideUnitCircle * spawnRadius;
			
			SpawnEntity(new Vector3(
				(int)randomSpawn.x + spawnCenter.x + 0.5f,
				1,
				(int)randomSpawn.y + spawnCenter.z + 0.5f));
		}
	}
}