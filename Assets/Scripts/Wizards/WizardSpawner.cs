using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Wizards
{
	public class WizardSpawner : MonoBehaviour
	{
		[Header("External Components")]
		[SerializeField] private WizardManager entityManager;

		[Header("Prefabs")]
		[SerializeField] private Wizard entityPrefab;

		[Header("Spawn Settings")] 
		[SerializeField] private int initialSpawns = 5;
		[SerializeField] private Vector3Int spawnCenter = Vector3Int.zero;
		[SerializeField] private int spawnRadius = 10;

		public void SpawnEntity(Vector3 spawnPosition)
		{
			Wizard wizard = Instantiate(entityPrefab, entityManager.transform);
			wizard.Transform.position = spawnPosition;
			
			wizard.InitializeWizard(NameGenerator.GetNewName(), GetRandomWizardType());
			
			entityManager.Add(wizard);
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

		private static WizardType GetRandomWizardType() => RandomExt.RandomEnumValue<WizardType>();
	}
}