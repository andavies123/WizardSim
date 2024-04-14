using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Wizards
{
	public class WizardSpawner : MonoBehaviour
	{
		[SerializeField] private WizardManager wizardManager;

		[Header("Prefabs")]
		[SerializeField] private GameObject wizardPrefab;

		[Header("Spawn Settings")] 
		[SerializeField] private int initialSpawns = 5;
		[SerializeField] private Vector3Int spawnCenter = Vector3Int.zero;
		[SerializeField] private int spawnRadius = 10;

		[Header("Game Events")]
		[SerializeField] private GameEventVector3 wizardSpawnRequestEvent;

		public void SpawnWizard(Vector3 spawnPosition)
		{
			GameObject wizard = Instantiate(wizardPrefab, wizardManager.transform);
			wizard.transform.position = spawnPosition;
			wizardManager.AddWizard(wizard.GetComponent<Wizard>());
		}

		private void Awake()
		{
			wizardSpawnRequestEvent.Raised += OnWizardSpawnRequestEvent;
		}

		private void Start()
		{
			LoopUtilities.Loop(initialSpawns, SpawnRandomWizard);
		}

		private void OnDestroy()
		{
			wizardSpawnRequestEvent.Raised += OnWizardSpawnRequestEvent;
		}

		private void SpawnRandomWizard()
		{
			Vector2 randomSpawn = Random.insideUnitCircle * spawnRadius;
			
			SpawnWizard(new Vector3(
				(int)randomSpawn.x + spawnCenter.x + 0.5f,
				1,
				(int)randomSpawn.y + spawnCenter.z + 0.5f));
		}

		private void OnWizardSpawnRequestEvent(Vector3 position) => SpawnWizard(position);
	}
}