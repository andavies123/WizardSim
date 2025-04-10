using System;
using Extensions;
using Game.Common;
using Game.Events;
using Game.Values;
using GameWorld;
using GameWorld.Characters.Wizards;
using GameWorld.Characters.Wizards.Managers;
using UnityEngine;
using Utilities;
using Utilities.Attributes;
using Random = UnityEngine.Random;

namespace Game
{
	/*
	 * Possible States:
	 * 
	 * - Force placement of town hall state
	 * - Pick upgrade state
	 * 
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
		private GameSpeed _gameSpeedBeforeUpgrade = GameSpeed.Regular;

		private void Awake()
		{
			_resourceLoader.LoadAllResources();
		}

		private void Start()
		{
			GameEvents.General.GameLoaded.Raised += OnGameLoaded;
			
			GameEvents.Settlement.TownHallPlaced.Raised += OnTownHallPlaced;

			GameEvents.Time.DaytimeStarted.Raised += OnDayStarted;
			GameEvents.Time.NighttimeStarted.Raised += OnNightStarted;
			
			GameEvents.UI.UpgradeSelected.Raised += OnUpgradeSelected;
		}

		private void OnDestroy()
		{
			GameEvents.General.GameLoaded.Raised -= OnGameLoaded;
			
			GameEvents.Settlement.TownHallPlaced.Raised -= OnTownHallPlaced;

			GameEvents.Time.DaytimeStarted.Raised -= OnDayStarted;
			GameEvents.Time.NighttimeStarted.Raised -= OnNightStarted;
			
			GameEvents.UI.UpgradeSelected.Raised -= OnUpgradeSelected;
		}

		private void OnGameLoaded(object sender, EventArgs args)
		{
			// Pause time while the player places the townhall
			GameEvents.Time.ChangeGameSpeed.Request(this, new GameSpeedEventArgs(GameSpeed.Paused));
			
			// Force the player to place a town hall
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
			_gameSpeedBeforeUpgrade = GameValues.Time.GameSpeed;
			GameEvents.Time.ChangeGameSpeed.Request(this, new GameSpeedEventArgs(GameSpeed.Paused));
			
			// Open the upgrade window
			GameEvents.UI.OpenUI.Request(this, new OpenUIEventArgs(UIWindow.UpgradeWindow));
		}

		private void OnNightStarted(object sender, EventArgs args)
		{
			// Todo: Start spawning enemies
		}

		private void OnUpgradeSelected(object sender, UpgradeSelectedEventArgs args)
		{
			// Close the upgrade window
			GameEvents.UI.CloseUI.Request(this, new CloseUIEventArgs(UIWindow.UpgradeWindow));
			
			// Resume time
			GameEvents.Time.ChangeGameSpeed.Request(this, new GameSpeedEventArgs(_gameSpeedBeforeUpgrade));
		}
	}
}