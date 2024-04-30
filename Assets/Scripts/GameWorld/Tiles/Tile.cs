using Game.MessengerSystem;
using GameWorld.Messages;
using UI;
using UI.ContextMenus;
using UnityEngine;

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
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Spawn Wizard", SpawnWizard));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Spawn Enemy", SpawnEnemy));
		}
		
		private void SpawnWizard()
		{
			Vector2 tileWorldPosition = ParentWorld.WorldPositionFromTile(this, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			GlobalMessenger.Publish(new WizardSpawnRequestMessage(spawnPosition));
		}

		private void SpawnEnemy()
		{
			Vector2 tileWorldPosition = ParentWorld.WorldPositionFromTile(this, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			GlobalMessenger.Publish(new EnemySpawnRequestMessage(spawnPosition));
		}
	}
}