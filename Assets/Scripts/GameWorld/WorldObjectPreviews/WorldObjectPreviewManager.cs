using System.Collections.Generic;
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
		private readonly List<ISubscription> _subscriptions = new();

		public WorldObjectPreviewManager(World world, Transform previewParent)
		{
			_world = world;
			_previewParent = previewParent;

			SubscriptionBuilder subscriptionBuilder = new(this);
			
			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<WorldObjectPreviewSetDetailsMessage>()
				.SetCallback(OnSetDetailsMessageReceived)
				.Build());
			
			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<WorldObjectPreviewSetPositionMessage>()
				.SetCallback(OnSetPositionMessageReceived)
				.Build());
			
			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<WorldObjectPreviewSetVisibilityMessage>()
				.SetCallback(OnSetVisibilityMessageReceived)
				.Build());
			
			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<WorldObjectPreviewDeleteMessage>()
				.SetCallback(OnDeletePreviewMessageReceived)
				.Build());
		}
		
		private WorldObject PreviewObject { get; set; }
		private WorldObjectDetails PreviewDetails { get; set; }
		private Vector3 PreviewWorldPosition { get; set; }
		private bool PreviewVisibility { get; set; }

		public void SubscribeToMessages()
		{
			_subscriptions.ForEach(MessageBroker.Subscribe);

			_world.WorldObjectManager.WorldObjectAdded += OnWorldObjectCountChanged;
			_world.WorldObjectManager.WorldObjectRemoved += OnWorldObjectCountChanged;
		}

		public void UnsubscribeFromMessages()
		{
			_subscriptions.ForEach(MessageBroker.Unsubscribe);

			_world.WorldObjectManager.WorldObjectAdded -= OnWorldObjectCountChanged;
			_world.WorldObjectManager.WorldObjectRemoved -= OnWorldObjectCountChanged;
		}

		private void OnSetDetailsMessageReceived(IMessage message)
		{
			if (message is not WorldObjectPreviewSetDetailsMessage setDetailsMessage || !setDetailsMessage.Details)
			{
				Debug.LogWarning($"Received invalid {nameof(WorldObjectPreviewSetDetailsMessage)}");
				return;
			}
			
			PreviewDetails = setDetailsMessage.Details;
			
			CreatePreview(PreviewDetails, _previewParent);
			PreviewObject.transform.SetPositionAndRotation(PreviewWorldPosition, Quaternion.identity);
			PreviewObject.gameObject.SetActive(PreviewVisibility);
		}

		private void OnSetPositionMessageReceived(IMessage message)
		{
			if (message is not WorldObjectPreviewSetPositionMessage setPositionMessage)
			{
				Debug.LogWarning($"Received invalid {nameof(WorldObjectPreviewSetPositionMessage)}");
				return;
			}
			
			PreviewWorldPosition = _world
				.WorldPositionFromTilePosition(setPositionMessage.TilePosition, setPositionMessage.ChunkPosition, centerOfTile: false)
				.ToVector3(VectorSub.XSubY);
			
			if (PreviewObject)
				PreviewObject.transform.SetPositionAndRotation(PreviewWorldPosition, Quaternion.identity);
		}

		private void OnSetVisibilityMessageReceived(IMessage message)
		{
			if (message is not WorldObjectPreviewSetVisibilityMessage setVisibilityMessage)
			{
				Debug.LogWarning($"Received invalid {nameof(WorldObjectPreviewSetVisibilityMessage)}");
				return;
			}

			PreviewVisibility = setVisibilityMessage.Visibility;
            
			if (PreviewObject)
				PreviewObject.gameObject.SetActive(PreviewVisibility);
		}

		private void OnDeletePreviewMessageReceived(IMessage message)
		{
			if (message is not WorldObjectPreviewDeleteMessage)
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