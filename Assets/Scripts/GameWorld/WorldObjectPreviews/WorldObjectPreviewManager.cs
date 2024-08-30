using System.Linq;
using Extensions;
using Game.MessengerSystem;
using GameWorld.WorldObjectPreviews.Messages;
using GameWorld.WorldObjects;
using GeneralBehaviours.ShaderManagers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameWorld.WorldObjectPreviews
{
	public class WorldObjectPreviewManager
	{
		private static readonly int BaseColorShaderId = Shader.PropertyToID("_BaseColor");

		private readonly World _world;
		private readonly Transform _previewParent;
		private WorldObject _previewObject;

		public WorldObjectPreviewManager(World world, Transform previewParent)
		{
			_world = world;
			_previewParent = previewParent;
		}
		
		private WorldObjectDetails PreviewDetails { get; set; }
		private Vector3 PreviewWorldPosition { get; set; }
		private bool PreviewVisibility { get; set; }

		public void SubscribeToMessages()
		{
			GlobalMessenger.Subscribe<WorldObjectPreviewSetDetailsMessage>(OnSetDetailsMessageReceived);
			GlobalMessenger.Subscribe<WorldObjectPreviewSetPositionMessage>(OnSetPositionMessageReceived);
			GlobalMessenger.Subscribe<WorldObjectPreviewSetVisibilityMessage>(OnSetVisibilityMessageReceived);
			GlobalMessenger.Subscribe<WorldObjectPreviewDeleteMessage>(OnDeletePreviewMessageReceived);
		}

		public void UnsubscribeFromMessages()
		{
			GlobalMessenger.Unsubscribe<WorldObjectPreviewSetDetailsMessage>(OnSetDetailsMessageReceived);
			GlobalMessenger.Unsubscribe<WorldObjectPreviewSetPositionMessage>(OnSetPositionMessageReceived);
			GlobalMessenger.Unsubscribe<WorldObjectPreviewSetVisibilityMessage>(OnSetVisibilityMessageReceived);
			GlobalMessenger.Unsubscribe<WorldObjectPreviewDeleteMessage>(OnDeletePreviewMessageReceived);
		}

		private void OnSetDetailsMessageReceived(WorldObjectPreviewSetDetailsMessage message)
		{
			if (!message?.Details)
			{
				Debug.LogWarning($"Received invalid {nameof(WorldObjectPreviewSetDetailsMessage)}");
				return;
			}
			
			PreviewDetails = message.Details;
			
			if (_previewObject)
				DeletePreview();
                
			_previewObject = CreatePreview(PreviewDetails, _previewParent);
			_previewObject.transform.SetPositionAndRotation(PreviewWorldPosition, Quaternion.identity);
			_previewObject.gameObject.SetActive(PreviewVisibility);
		}

		private void OnSetPositionMessageReceived(WorldObjectPreviewSetPositionMessage message)
		{
			if (message == null)
			{
				Debug.LogWarning($"Received invalid {nameof(WorldObjectPreviewSetPositionMessage)}");
				return;
			}
			
			PreviewWorldPosition = _world
				.WorldPositionFromTilePosition(message.TilePosition, message.ChunkPosition, centerOfTile: false)
				.ToVector3(VectorSub.XSubY);
			
			if (_previewObject)
				_previewObject.transform.SetPositionAndRotation(PreviewWorldPosition, Quaternion.identity);
		}

		private void OnSetVisibilityMessageReceived(WorldObjectPreviewSetVisibilityMessage message)
		{
			if (message == null)
			{
				Debug.LogWarning($"Received invalid {nameof(WorldObjectPreviewSetVisibilityMessage)}");
				return;
			}

			PreviewVisibility = message.Visibility;
            
			if (_previewObject)
				_previewObject.gameObject.SetActive(PreviewVisibility);
		}

		private void OnDeletePreviewMessageReceived(WorldObjectPreviewDeleteMessage message)
		{
			if (message == null)
			{
				Debug.LogWarning($"Received invalid {nameof(WorldObjectPreviewDeleteMessage)}");
				return;
			}
            
			DeletePreview();
		}

		private void DeletePreview()
		{
			if (_previewObject)
			{
				_previewObject.gameObject.Destroy();
				_previewObject = null;
			}
		}
		
		private static WorldObject CreatePreview(WorldObjectDetails worldObjectDetails, Transform parent)
		{
			WorldObject worldObject = Object.Instantiate(worldObjectDetails.Prefab, parent);

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