using System;
using AndysTools.GameWorldTimeManagement.Runtime;
using GeneralBehaviours.ShaderManagers;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace GameWorld.Characters.Wizards
{
	public class WizardSpawner : MonoBehaviour
	{
		[Header("External Components")]
		[SerializeField] private WizardManager wizardManager;
		[SerializeField] private GameWorldTimeBehaviour gameWorldTime;

		[Header("Prefabs")]
		[SerializeField] private Wizard entityPrefab;

		[Header("Spawn Settings")] 
		[SerializeField] private int initialSpawns = 5;
		[SerializeField] private Vector3Int spawnCenter = Vector3Int.zero;
		[SerializeField] private int spawnRadius = 10;

		[Header("Wizard Colors")] 
		[SerializeField] private Color fireWizardColor;
		[SerializeField] private Color earthWizardColor;
		[SerializeField] private Color waterWizardColor;
		[SerializeField] private Color lightningWizardColor;

		public void SpawnWizard(Vector3 spawnPosition, WizardType wizardType)
		{
			Wizard wizard = Instantiate(entityPrefab, wizardManager.transform);
			wizard.Transform.position = spawnPosition;

			wizard.InitializeWizard(NameGenerator.GetNewName(), wizardType, gameWorldTime);
			wizard.GetComponent<InteractionShaderManager>().OverrideBaseColor(GetColorFromWizardType(wizardType));
			
			wizardManager.Add(wizard);
		}
		
		private void Start()
		{
			LoopUtilities.Loop(initialSpawns, SpawnRandomEntity);
		}

		private void SpawnRandomEntity()
		{
			Vector2 randomSpawn = Random.insideUnitCircle * spawnRadius;
			
			SpawnWizard(
				new Vector3((int)randomSpawn.x + spawnCenter.x + 0.5f, 1,(int)randomSpawn.y + spawnCenter.z + 0.5f),
				GetRandomWizardType());
		}

		private static WizardType GetRandomWizardType()
		{
			return RandomExt.RandomEnumValue<WizardType>();
		}

		private Color GetColorFromWizardType(WizardType wizardType)
		{
			return wizardType switch
			{
				WizardType.Earth => earthWizardColor,
				WizardType.Water => waterWizardColor,
				WizardType.Fire => fireWizardColor,
				WizardType.Lightning => lightningWizardColor,
				_ => throw new ArgumentOutOfRangeException(nameof(wizardType), wizardType, null)
			};
		}
	}
}