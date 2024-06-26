﻿using System;
using GeneralBehaviours.ShaderManagers;
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

		[Header("Wizard Colors")] 
		[SerializeField] private Color fireWizardColor;
		[SerializeField] private Color earthWizardColor;
		[SerializeField] private Color waterWizardColor;
		[SerializeField] private Color lightningWizardColor;

		public void SpawnEntity(Vector3 spawnPosition)
		{
			Wizard wizard = Instantiate(entityPrefab, entityManager.transform);
			wizard.Transform.position = spawnPosition;

			WizardType wizardType = GetRandomWizardType();
			wizard.InitializeWizard(NameGenerator.GetNewName(), wizardType);
			wizard.GetComponent<InteractionShaderManager>().OverrideBaseColor(GetColorFromWizardType(wizardType));
			
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