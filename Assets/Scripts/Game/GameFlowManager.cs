using System;
using Extensions;
using Game.Events;
using GameWorld;
using GameWorld.Characters.Wizards;
using GameWorld.Characters.Wizards.Managers;
using UnityEngine;
using Utilities;
using Utilities.Attributes;
using Random = UnityEngine.Random;

namespace Game
{
	/* Possible States:
	 * 
	 * - Force placement of town hall state
	 * - Pick upgrade state
	 */
	[DisallowMultipleComponent]
	public class GameFlowManager : MonoBehaviour
	{
		[SerializeField, Required] private World world;
		[SerializeField, Required] private WizardController wizardController;

		[Header("On Town Hall Placed")]
		[SerializeField] private int initialWizardSpawns = 3;
		[SerializeField] private float spawnDistanceFromTownHall = 5;

		private readonly ResourceLoader _resourceLoader = new();

		private bool _townHallInitiallyPlaced;

		private void Awake()
		{
			_resourceLoader.LoadAllResources();
		}

		private void Start()
		{
			GameEvents.Settlement.TownHallPlaced.Raised += OnTownHallPlaced;

			GameEvents.Time.DaytimeStarted.Raised += OnDayStarted;
			GameEvents.Time.NighttimeStarted.Raised += OnNightStarted;
			
			GameEvents.UI.UpgradeSelected.Raised += OnUpgradeSelected;
		}

		private void OnDestroy()
		{
			GameEvents.Settlement.TownHallPlaced.Raised -= OnTownHallPlaced;

			GameEvents.Time.DaytimeStarted.Raised -= OnDayStarted;
			GameEvents.Time.NighttimeStarted.Raised -= OnNightStarted;
			
			GameEvents.UI.UpgradeSelected.Raised -= OnUpgradeSelected;
		}

		private void OnTownHallPlaced(object sender, TownHallPlacedEventArgs args)
		{
			if (!_townHallInitiallyPlaced && args.TownHall)
			{
				_townHallInitiallyPlaced = true;
				Vector3 townHallCenter = world.Settlement.TownHall.WorldObject.PositionDetails.Center.SubY(0);
				for (int i = 0; i < initialWizardSpawns; i++)
				{
					Vector3 randomPosition = Random.insideUnitCircle.ToVector3(VectorSub.XSubY);
					Vector3 spawnPosition = randomPosition * spawnDistanceFromTownHall + townHallCenter;
					//wizardFactory.CreateNewWizard(spawnPosition + Vector3.up, RandomExt.RandomEnumValue<WizardType>());
					
					wizardController.SpawnWizard(spawnPosition, RandomExt.RandomEnumValue<WizardType>());
				}
			}
		}

		private void OnDayStarted(object sender, EventArgs args)
		{
			// Time should be paused before selecting upgrade
			GameEvents.Time.ChangeGameSpeed.Request(this, new GameSpeedEventArgs {GameSpeed = GameSpeed.Paused});
			
			// Open the upgrade window
			GameEvents.UI.OpenUI.Request(this, new OpenUIEventArgs {Window = UIWindow.UpgradeWindow});
		}

		private void OnNightStarted(object sender, EventArgs args)
		{
			// Todo: Start spawning enemies
		}

		private void OnUpgradeSelected(object sender, UpgradeSelectedEventArgs args)
		{
			// Close the upgrade window
			GameEvents.UI.CloseUI.Request(this, new CloseUIEventArgs {Window = UIWindow.UpgradeWindow});
			
			// Resume time
			GameEvents.Time.ChangeGameSpeed.Request(this, new GameSpeedEventArgs {GameSpeed = GameSpeed.Regular});
		}
	}
}