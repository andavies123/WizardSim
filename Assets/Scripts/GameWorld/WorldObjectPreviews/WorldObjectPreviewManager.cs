using System;
using System.Linq;
using Extensions;
using Game;
using Game.Events;
using GameWorld.WorldObjects;
using GeneralBehaviours.ShaderManagers;
using UnityEngine;
using static Game.Events.GameWorldEvents;
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
			_world.WorldObjectManager.WorldObjectAdded += OnWorldObjectCountChanged;
			_world.WorldObjectManager.WorldObjectRemoved += OnWorldObjectCountChanged;

			GameEvents.GameWorld.PlacePreviewWorldObject.Requested += OnPlacePreviewRequested;
			GameEvents.GameWorld.DeletePreviewWorldObject.Requested += OnDeletePreviewRequested;
		}

		public void UnsubscribeFromMessages()
		{
			_world.WorldObjectManager.WorldObjectAdded -= OnWorldObjectCountChanged;
			_world.WorldObjectManager.WorldObjectRemoved -= OnWorldObjectCountChanged;

			GameEvents.GameWorld.PlacePreviewWorldObject.Requested -= OnPlacePreviewRequested;
			GameEvents.GameWorld.DeletePreviewWorldObject.Requested -= OnDeletePreviewRequested;
		}

		private void OnPlacePreviewRequested(object sender, PlacementEventArgs args)
		{
			Vector3 newPreviewWorldPosition = _world
				.WorldPositionFromTilePosition(args.TilePosition, args.ChunkPosition, centerOfTile: false)
				.ToVector3(VectorSub.XSubY);

			if (PreviewWorldPosition != newPreviewWorldPosition)
			{
				PreviewWorldPosition = newPreviewWorldPosition;
			}

			if (PreviewVisibility != args.Visibility)
			{
				PreviewVisibility = args.Visibility;
			}
			
			if (PreviewDetails != args.WorldObjectDetails)
			{
				PreviewDetails = args.WorldObjectDetails;
				UpdatePreviewDetails();
			}
			else // Updating the details already sets the position/visibility due to it being a new object
			{
				UpdatePreviewPosition();
				UpdatePreviewVisibility();	
			}
		}

		private void OnDeletePreviewRequested(object sender, EventArgs args)
		{
			DeletePreview();
		}

		private void UpdatePreviewDetails()
		{
			CreatePreview(PreviewDetails, _previewParent);
			PreviewObject.transform.SetPositionAndRotation(PreviewWorldPosition, Quaternion.identity);
			PreviewObject.gameObject.SetActive(PreviewVisibility);
		}

		private void UpdatePreviewPosition()
		{
			if (PreviewObject)
			{
				PreviewObject.transform.SetPositionAndRotation(PreviewWorldPosition, Quaternion.identity);
			}
		}

		private void UpdatePreviewVisibility()
		{
			if (PreviewObject)
			{
				PreviewObject.gameObject.SetActive(PreviewVisibility);
			}
		}

		private void OnWorldObjectCountChanged(object sender, WorldObjectManagerEventArgs args)
		{
			if (args.Details != PreviewDetails)
				return; // We only care about the current detail set

			UpdatePreviewColor();
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

		private void DeletePreview()
		{
			if (PreviewObject)
			{
				PreviewObject.gameObject.Destroy();
				PreviewObject = null;
			}
		}
	}
}