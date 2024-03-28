using GameWorld;
using UnityEngine;
using Utilities;

namespace Wizards
{
	public class WizardSpawner : MonoBehaviour
	{
		[SerializeField] private WizardManager wizardManager;
		[SerializeField] private World world;

		[Header("Prefabs")]
		[SerializeField] private GameObject wizardPrefab;

		[Header("Spawn Settings")] 
		[SerializeField] private int initialSpawns = 5;
		[SerializeField] private Vector3Int spawnCenter = Vector3Int.zero;
		[SerializeField] private int spawnRadius = 10;

		private void Start()
		{
			LoopUtilities.Loop(initialSpawns, SpawnWizard);
		}

		private void SpawnWizard()
		{
			GameObject wizard = Instantiate(wizardPrefab, wizardManager.transform);

			Vector2 randomSpawn = Random.insideUnitCircle * spawnRadius;
			wizard.transform.position = new Vector3(
				(int)randomSpawn.x + spawnCenter.x + 0.5f,
				1, 
				(int)randomSpawn.y + spawnCenter.z + 0.5f);
		}
	}
}