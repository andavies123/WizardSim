using Game;
using GameWorld.Builders;
using UI;
using UI.ContextMenus;
using UnityEngine;
using Utilities;

namespace GameWorld.Tiles
{
	[RequireComponent(typeof(Interactable), typeof(ContextMenuUser))]
	public class Tile : MonoBehaviour
	{
		[SerializeField] private GameEventVector3 wizardSpawnRequest;
		[SerializeField] private GameEventVector3 enemySpawnRequest;
		
		private Interactable _interactable;
		private ContextMenuUser _contextMenuUser;
		private WorldBuilder _worldBuilder;
		
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
			_worldBuilder = Dependencies.GetDependency<WorldBuilder>();
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
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Spawn Rock", SpawnRock));
		}
		
		private void SpawnWizard()
		{
			Vector2 tileWorldPosition = ParentWorld.WorldPositionFromTile(this, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			wizardSpawnRequest.RaiseEvent(this, spawnPosition);
		}

		private void SpawnEnemy()
		{
			Vector2 tileWorldPosition = ParentWorld.WorldPositionFromTile(this, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			enemySpawnRequest.RaiseEvent(this, spawnPosition);
		}

		private void SpawnRock()
		{
			_worldBuilder.RockWorldBuilder.TrySpawnSingle(ParentChunk, TilePosition);
		}
	}
}