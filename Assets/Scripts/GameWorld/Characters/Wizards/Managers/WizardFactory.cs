﻿using System;
using AndysTools.GameWorldTimeManagement.Runtime;
using GameWorld.Settlements;
using GeneralBehaviours.ShaderManagers;
using UnityEngine;
using Utilities;
using Utilities.Attributes;
using Random = UnityEngine.Random;

namespace GameWorld.Characters.Wizards.Managers
{
	internal class WizardFactory : MonoBehaviour
	{
		[Header("External Components")]
		[SerializeField, Required] private Settlement settlement;
		[SerializeField, Required] private GameWorldTimeBehaviour gameWorldTime;

		[Header("Prefabs")]
		[SerializeField, Required] private Wizard wizardPrefab;

		[Header("Spawn Settings")] 
		[SerializeField] private int initialSpawns = 5;
		[SerializeField] private Vector3Int spawnCenter = Vector3Int.zero;
		[SerializeField] private int spawnRadius = 10;

		[Header("Wizard Colors")]
		[SerializeField] private Color undecidedWizardColor;
		[SerializeField] private Color fireWizardColor;
		[SerializeField] private Color earthWizardColor;
		[SerializeField] private Color waterWizardColor;
		[SerializeField] private Color lightningWizardColor;

		public Wizard CreateNewWizard(Vector3 spawnPosition, WizardType wizardType)
		{
			Wizard wizard = Instantiate(wizardPrefab, transform, true);
			SetWizardType(wizard, wizardType);
			wizard.Transform.position = spawnPosition;
			wizard.Attributes = new WizardAttributes
			{
				Strength = { CurrentLevel = Random.Range(0, 5) },
				Endurance = { CurrentLevel = Random.Range(0, 5) },
				Vitality = { CurrentLevel = Random.Range(0, 5) },
				Magic = { CurrentLevel = Random.Range(0, 5) },
				Mana = { CurrentLevel = Random.Range(0, 5) },
				Intelligence = { CurrentLevel = Random.Range(0, 5) },
				Courage = { CurrentLevel = Random.Range(0, 5) }
			};
			wizard.InitializeWizard(NameGenerator.GetNewName(), settlement, gameWorldTime);
			return wizard;
		}

		public void SetWizardType(Wizard wizard, WizardType wizardType)
		{
			wizard.SetWizardType(wizardType);
			wizard.GetComponent<InteractionShaderManager>().OverrideBaseColor(GetColorFromWizardType(wizardType));
		}
		
		private void Start()
		{
			LoopUtilities.Loop(initialSpawns, SpawnRandomWizard);
		}

		private void SpawnRandomWizard()
		{
			Vector2 randomSpawn = Random.insideUnitCircle * spawnRadius;
			float xPos = (int) randomSpawn.x + spawnCenter.x + 0.5f;
			float zPos = (int) randomSpawn.y + spawnCenter.z + 0.5f;
			Vector3 spawnPosition = new(xPos, 1, zPos);
			
			CreateNewWizard(spawnPosition, GetRandomWizardType());
		}

		private Color GetColorFromWizardType(WizardType wizardType)
		{
			return wizardType switch
			{
				WizardType.Undecided => undecidedWizardColor,
				WizardType.Earth => earthWizardColor,
				WizardType.Water => waterWizardColor,
				WizardType.Fire => fireWizardColor,
				WizardType.Lightning => lightningWizardColor,
				_ => throw new ArgumentOutOfRangeException(nameof(wizardType), wizardType, null)
			};
		}
		
		private static WizardType GetRandomWizardType() => RandomExt.RandomEnumValue<WizardType>();
	}
}