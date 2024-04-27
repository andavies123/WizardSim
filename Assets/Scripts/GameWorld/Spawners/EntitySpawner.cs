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

		[Header("Game Events")]
		[SerializeField] private GameEventVector3 spawnRequestEvent;

		public void SpawnEntity(Vector3 spawnPosition)
		{
			GameObject entityGameObject = Instantiate(entityPrefab, entityManager.transform);
			entityGameObject.transform.position = spawnPosition;
			Entity entity = entityGameObject.GetComponent<Entity>();
			entityManager.Add(entity);
		}

		private void Awake()
		{
			spawnRequestEvent.Raised += OnEntitySpawnRequestEvent;
		}

		private void Start()
		{
			LoopUtilities.Loop(initialSpawns, SpawnRandomEntity);
		}

		private void OnDestroy()
		{
			spawnRequestEvent.Raised += OnEntitySpawnRequestEvent;
		}

		private void SpawnRandomEntity()
		{
			Vector2 randomSpawn = Random.insideUnitCircle * spawnRadius;
			
			SpawnEntity(new Vector3(
				(int)randomSpawn.x + spawnCenter.x + 0.5f,
				1,
				(int)randomSpawn.y + spawnCenter.z + 0.5f));
		}

		private void OnEntitySpawnRequestEvent(object sender, Vector3 position) => SpawnEntity(position);
	}
}