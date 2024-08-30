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
		private const float PREVIEW_ALPHA_VALUE = 0.4f;

		private static readonly Color DefaultPreviewColor = new(0.5f, 0.5f, 0.5f, PREVIEW_ALPHA_VALUE);
		private static readonly Color MaxObjectCountPreviewColor = new(0.75f, 0, 0, PREVIEW_ALPHA_VALUE);
		private static readonly int BaseColorShaderID = Shader.PropertyToID("_BaseColor");

		private readonly World _world;
		private readonly Transform _previewParent;

		public WorldObjectPreviewManager(World world, Transform previewParent)
		{
			_world = world;
			_previewParent = previewParent;
		}
		
		private WorldObject PreviewObject { get; set; }
		private WorldObjectDetails PreviewDetails { get; set; }
		private Vector3 PreviewWorldPosition { get; set; }
		private bool PreviewVisibility { get; set; }

		public void SubscribeToMessages()
		{
			GlobalMessenger.Subscribe<WorldObjectPreviewSetDetailsMessage>(OnSetDetailsMessageReceived);
			GlobalMessenger.Subscribe<WorldObjectPreviewSetPositionMessage>(OnSetPositionMessageReceived);
			GlobalMessenger.Subscribe<WorldObjectPreviewSetVisibilityMessage>(OnSetVisibilityMessageReceived);
			GlobalMessenger.Subscribe<WorldObjectPreviewDeleteMessage>(OnDeletePreviewMessageReceived);

			_world.WorldObjectManager.WorldObjectAdded += OnWorldObjectCountChanged;
			_world.WorldObjectManager.WorldObjectRemoved += OnWorldObjectCountChanged;
		}

		public void UnsubscribeFromMessages()
		{
			GlobalMessenger.Unsubscribe<WorldObjectPreviewSetDetailsMessage>(OnSetDetailsMessageReceived);
			GlobalMessenger.Unsubscribe<WorldObjectPreviewSetPositionMessage>(OnSetPositionMessageReceived);
			GlobalMessenger.Unsubscribe<WorldObjectPreviewSetVisibilityMessage>(OnSetVisibilityMessageReceived);
			GlobalMessenger.Unsubscribe<WorldObjectPreviewDeleteMessage>(OnDeletePreviewMessageReceived);

			_world.WorldObjectManager.WorldObjectAdded -= OnWorldObjectCountChanged;
			_world.WorldObjectManager.WorldObjectRemoved -= OnWorldObjectCountChanged;
		}

		private void OnSetDetailsMessageReceived(WorldObjectPreviewSetDetailsMessage message)
		{
			if (!message?.Details)
			{
				Debug.LogWarning($"Received invalid {nameof(WorldObjectPreviewSetDetailsMessage)}");
				return;
			}
			
			PreviewDetails = message.Details;
			
			CreatePreview(PreviewDetails, _previewParent);
			PreviewObject.transform.SetPositionAndRotation(PreviewWorldPosition, Quaternion.identity);
			PreviewObject.gameObject.SetActive(PreviewVisibility);
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
			
			if (PreviewObject)
				PreviewObject.transform.SetPositionAndRotation(PreviewWorldPosition, Quaternion.identity);
		}

		private void OnSetVisibilityMessageReceived(WorldObjectPreviewSetVisibilityMessage message)
		{
			if (message == null)
			{
				Debug.LogWarning($"Received invalid {nameof(WorldObjectPreviewSetVisibilityMessage)}");
				return;
			}

			PreviewVisibility = message.Visibility;
            
			if (PreviewObject)
				PreviewObject.gameObject.SetActive(PreviewVisibility);
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

		private void OnWorldObjectCountChanged(object sender, WorldObjectManagerEventArgs args)
		{
			if (args.Details != PreviewDetails)
				return; // We only care about the current detail set

			UpdatePreviewColor();
		}

		private void DeletePreview()
		{
			if (PreviewObject)
			{
				PreviewObject.gameObject.Destroy();
				PreviewObject = null;
			}
		}

		private void UpdatePreviewColor()
		{
			if (!PreviewObject)
				return;

			Color previewColor = _world.WorldObjectManager.IsAtMaxCapacity(PreviewDetails) ? MaxObjectCountPreviewColor : DefaultPreviewColor;
			
			PreviewObject.GetComponentsInChildren<MeshRenderer>(true).ToList().ForEach(renderer =>
			{
				renderer.material.SetColor(BaseColorShaderID, previewColor);
			});
		}
		
		private void CreatePreview(WorldObjectDetails worldObjectDetails, Transform parent)
		{
			if (PreviewObject)
				DeletePreview();

			PreviewObject = Object.Instantiate(worldObjectDetails.Prefab, parent);

			PreviewObject.GetComponent<InteractionShaderManager>().enabled = false;
			PreviewObject.GetComponentsInChildren<Collider>(true).ToList().ForEach(collider => collider.enabled = false);
			UpdatePreviewColor();
		}
	}
}