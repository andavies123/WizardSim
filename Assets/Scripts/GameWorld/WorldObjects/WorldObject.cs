using System;
using Extensions;
using GeneralBehaviours.HealthBehaviours;
using GeneralBehaviours.ShaderManagers;
using UI;
using UI.ContextMenus;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Interactable))]
	[RequireComponent(typeof(InteractionShaderManager))]
	[RequireComponent(typeof(ContextMenuUser))]
	public abstract class WorldObject : MonoBehaviour
	{
		private WorldObjectProperties _worldObjectProperties;
		
		public HealthComponent Health { get; private set; }
		public Interactable Interactable { get; private set; }
		public ContextMenuUser ContextMenuUser { get; private set; }
		public ChunkPlacementData ChunkPlacementData { get; private set; }

		public abstract Vector3Int Size { get; } // Size in world tiles
		public abstract Vector3 InitialPositionOffset { get; } // Offset in world space
		protected abstract string ItemName { get; } // Name for loading properties
		
		public event EventHandler Destroyed;

		public void Init(ChunkPlacementData chunkPlacementData) => ChunkPlacementData = chunkPlacementData;

		protected abstract void InitializeContextMenu();
		
		protected virtual void Awake()
		{
			Interactable = gameObject.GetOrAddComponent<Interactable>();
			ContextMenuUser = gameObject.GetOrAddComponent<ContextMenuUser>();
		}

		protected virtual void Start()
		{
			LoadProperties();
			Interactable.InitializeWithProperties(_worldObjectProperties.InteractableProperties);
			InitializeContextMenu();
		}

		protected virtual void OnDestroy()
		{
			Destroyed?.Invoke(this, EventArgs.Empty);
		}

		private void LoadProperties()
		{
			if (!PropertiesLoader.WorldObjectPropertiesMap.TryGetValue(ItemName, out _worldObjectProperties))
			{
				Debug.LogWarning($"World Object - {ItemName} - was not found. Deleting {gameObject.name}...", this);
				gameObject.Destroy();
				return;
			}
			
			if (_worldObjectProperties.HealthProperties != null)
			{
				Health = gameObject.AddComponent<HealthComponent>();
				Health.InitializeWithProperties(_worldObjectProperties.HealthProperties);
			}

			gameObject.name = _worldObjectProperties.Id;
		}
	}

	public struct ChunkPlacementData
	{
		public Vector2Int ChunkPosition { get; }
		public Vector2Int TilePosition { get; }
        
		public ChunkPlacementData(Vector2Int chunkPosition, Vector2Int tilePosition)
		{
			ChunkPosition = chunkPosition;
			TilePosition = tilePosition;
		}
	}
}