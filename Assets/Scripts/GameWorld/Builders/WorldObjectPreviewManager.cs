using System;
using System.Linq;
using Extensions;
using Game.Messages;
using Game.MessengerSystem;
using GameWorld.Messages;
using GameWorld.WorldObjects;
using GeneralBehaviours.ShaderManagers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameWorld.Builders
{
	public class WorldObjectPreviewManager
	{
		private static readonly int BaseColorShaderId = Shader.PropertyToID("_BaseColor");

		private readonly World _world;
		private readonly Transform _previewParent;
		private WorldObject _previewObject;

		public event EventHandler<CreateWorldObjectRequestEventArgs> CreateWorldObjectRequested;

		public WorldObjectPreviewManager(World world, Transform previewParent)
		{
			_world = world;
			_previewParent = previewParent;
		}

		public void SubscribeToMessages()
		{
			GlobalMessenger.Subscribe<WorldObjectPreviewRequest>(OnPreviewRequested);
			GlobalMessenger.Subscribe<WorldObjectPlacementRequest>(OnPlacementRequested);
			GlobalMessenger.Subscribe<WorldObjectHidePreviewRequest>(OnHidePreviewRequested);
			GlobalMessenger.Subscribe<WorldObjectPreviewDeleteRequest>(OnPreviewDeleteRequested);
		}

		public void UnsubscribeFromMessages()
		{
			GlobalMessenger.Unsubscribe<WorldObjectPreviewRequest>(OnPreviewRequested);
			GlobalMessenger.Unsubscribe<WorldObjectPlacementRequest>(OnPlacementRequested);
			GlobalMessenger.Unsubscribe<WorldObjectHidePreviewRequest>(OnHidePreviewRequested);
			GlobalMessenger.Unsubscribe<WorldObjectPreviewDeleteRequest>(OnPreviewDeleteRequested);
		}
		
		private void OnPreviewRequested(WorldObjectPreviewRequest message)
		{
			if (!_previewObject)
				_previewObject = CreatePreview(message.WorldObjectPrefab, _previewParent);
			
			Vector3 worldPosition = _world
				.WorldPositionFromTilePosition(message.TilePosition, message.ChunkPosition, centerOfTile: false)
				.ToVector3(VectorSub.XSubY) + _previewObject.InitialPositionOffset;
			_previewObject.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);

			// Make sure the game object is active as it might have been disabled due to the hide request
			_previewObject.gameObject.SetActive(true);
		}
        
		private void OnPlacementRequested(WorldObjectPlacementRequest message)
		{
			CreateWorldObjectRequested?.Invoke(this, new CreateWorldObjectRequestEventArgs
			{
				WorldObjectPrefab = message.WorldObjectPrefab,
				ChunkPosition = message.ChunkPosition,
				TilePosition = message.TilePosition
			});
		}

		private void OnHidePreviewRequested(WorldObjectHidePreviewRequest message)
		{
			if (_previewObject)
			{
				_previewObject.gameObject.SetActive(false);
			}
		}

		private void OnPreviewDeleteRequested(WorldObjectPreviewDeleteRequest message)
		{
			if (_previewObject)
			{
				_previewObject.gameObject.Destroy();
				_previewObject = null;
			}
		}
		
		private static WorldObject CreatePreview(GameObject prefab, Transform parent)
		{
			WorldObject worldObject = Object.Instantiate(prefab, parent).GetComponent<WorldObject>();

			worldObject.GetComponent<InteractionShaderManager>().enabled = false;
			worldObject.GetComponentsInChildren<Collider>(true).ToList().ForEach(collider => collider.enabled = false);
			worldObject.GetComponentsInChildren<MeshRenderer>(true).ToList().ForEach(renderer =>
			{
				Color color = renderer.material.GetColor(BaseColorShaderId);
				color.a = 0.75f;
				renderer.material.SetColor(BaseColorShaderId, color);
			});

			return worldObject;
		}
	}
}