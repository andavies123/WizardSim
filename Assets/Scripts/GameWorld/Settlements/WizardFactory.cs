using System;
using AndysTools.GameWorldTimeManagement.Runtime;
using Extensions;
using Game;
using GameWorld.Characters.Wizards;
using GameWorld.Settlements.Interfaces;
using GameWorld.Tiles;
using GeneralBehaviours.ShaderManagers;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using UI.ContextMenus;
using UnityEngine;
using Utilities;
using Utilities.Attributes;
using Random = UnityEngine.Random;

namespace GameWorld.Settlements
{
	public class WizardFactory : MonoBehaviour, IWizardFactory, IContextMenuUser
	{
		[Header("External Components")]
		[SerializeField, Required] private Settlement settlement;
		[SerializeField] private GameWorldTimeBehaviour gameWorldTime;

		[Header("Prefabs")]
		[SerializeField] private Wizard entityPrefab;

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

		private IWizardRepo _wizardRepo;

		public void Initialize(IWizardRepo wizardRepo)
		{
			_wizardRepo = wizardRepo.ThrowIfNull(nameof(wizardRepo));
		}

		public void CreateNewWizard(Vector3 spawnPosition, WizardType wizardType)
		{
			Wizard wizard = Instantiate(entityPrefab);
			wizard.Transform.position = spawnPosition;
			wizard.WizardType = wizardType;
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
			wizard.GetComponent<InteractionShaderManager>().OverrideBaseColor(GetColorFromWizardType(wizardType));

			if (!_wizardRepo.TryAddWizard(wizard))
			{
				Debug.LogWarning("Unable to add wizard to wizard repository... Destroying wizard");
				wizard.gameObject.Destroy();
			}
		}
		
		private void Start()
		{
			InitializeContextMenu();
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

		private void InitializeContextMenu()
		{
			foreach (WizardType wizardType in Enum.GetValues(typeof(WizardType)))
			{
				Globals.ContextMenuInjections.InjectContextMenuOption<Tile>(
					ContextMenuBuilder.BuildPath("Spawn Wizard", wizardType.ToString()),
					tile => SpawnWizard(tile, wizardType),
					() => true,
					() => true);
			}
		}

		private void SpawnWizard(Tile tile, WizardType wizardType)
		{
			Vector2 tileWorldPosition = Globals.World.WorldPositionFromTile(tile, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			CreateNewWizard(spawnPosition, wizardType);
		}
		
		private static WizardType GetRandomWizardType() => RandomExt.RandomEnumValue<WizardType>();
	}
}