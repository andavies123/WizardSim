using Game.MessengerSystem;
using GameWorld.Messages;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using UI;
using UI.ContextMenus;
using UnityEngine;
using Wizards;

namespace GameWorld.Tiles
{
	[RequireComponent(typeof(Interactable), typeof(ContextMenuUser))]
	public class Tile : MonoBehaviour
	{
		private Interactable _interactable;
		private ContextMenuUser _contextMenuUser;
		
		public World ParentWorld { get; private set; }
		public Chunk ParentChunk { get; private set; }
		public Transform Transform { get; private set; }
		public Vector2Int TilePosition { get; private set; }

		public void Initialize(World parentWorld, Chunk parentChunk, Vector2Int tilePosition)
		{
			ParentWorld = parentWorld;
			ParentChunk = parentChunk;
			TilePosition = tilePosition;

			gameObject.name = $"Tile - {tilePosition}";
			
			InitializeInteractable();
			InitializeContextMenu();
		}

		private void Awake()
		{
			Transform = transform;
			_interactable = GetComponent<Interactable>();
			_contextMenuUser = GetComponent<ContextMenuUser>();
		}

		private void InitializeInteractable()
		{
			_interactable.TitleText = "Ground";
			_interactable.InfoText = $"{TilePosition}";
		}
		
		private void InitializeContextMenu()
		{
			_contextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Spawn Wizard", "Earth"),
				() => SpawnWizard(WizardType.Earth),
				() => true,
				() => true);
			
			_contextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Spawn Wizard", "Fire"),
				() => SpawnWizard(WizardType.Fire),
				() => true,
				() => true);
			
			_contextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Spawn Wizard", "Lightning"),
				() => SpawnWizard(WizardType.Lightning),
				() => true,
				() => true);
			
			_contextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Spawn Wizard", "Water"),
				() => SpawnWizard(WizardType.Water),
				() => true,
				() => true);
			
			_contextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Spawn Enemy"),
				SpawnEnemy,
				() => true,
				() => true);
		}
		
		private void SpawnWizard(WizardType wizardType)
		{
			Vector2 tileWorldPosition = ParentWorld.WorldPositionFromTile(this, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			GlobalMessenger.Publish(new WizardSpawnRequestMessage
			{
				Sender = this,
				SpawnPosition = spawnPosition,
				WizardType = wizardType
			});
		}

		private void SpawnEnemy()
		{
			Vector2 tileWorldPosition = ParentWorld.WorldPositionFromTile(this, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			GlobalMessenger.Publish(new EnemySpawnRequestMessage
			{
				Sender = this,
				SpawnPosition = spawnPosition
			});
		}
	}
}