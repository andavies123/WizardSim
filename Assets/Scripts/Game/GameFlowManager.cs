using Extensions;
using GameWorld;
using GameWorld.Characters.Wizards;
using GameWorld.Characters.Wizards.Managers;
using GameWorld.WorldObjects;
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

		private void Awake()
		{
			_resourceLoader.LoadAllResources();
		}

		private void Start()
		{
			world.Settlement.TownHallUpdated += OnTownHallUpdated;
		}

		private void OnDestroy()
		{
			world.Settlement.TownHallUpdated -= OnTownHallUpdated;
		}

		private void OnTownHallUpdated(TownHall townHall)
		{
			if (townHall)
			{
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
	}
}