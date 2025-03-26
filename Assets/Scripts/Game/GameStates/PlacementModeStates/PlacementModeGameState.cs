using System;
using CameraComponents;
using Extensions;
using GameWorld.GameWorldEventArgs;
using GameWorld.WorldObjects;
using UnityEngine;
using static Game.GameWorldEvents;

namespace Game.GameStates.PlacementModeStates
{
	public class PlacementModeGameState : GameState
	{
		private readonly PlacementModeInputState _placementModeInputState;
		private readonly PlacementModeUIState _placementModeUIState;

		private Vector2Int _latestPreviewChunkPosition;
		private Vector2Int _latestPreviewTilePosition;
		private bool _latestPreviewVisibility = true;
		
		public event EventHandler PlacementModeEnded
		{
			add => _placementModeInputState.EndPlacementModeActionPerformed += value;
			remove => _placementModeInputState.EndPlacementModeActionPerformed -= value;
		}
		
		public PlacementModeGameState(PlacementModeUIState placementModeUIState, InteractableRaycaster interactableRaycaster)
		{
			_placementModeInputState = new PlacementModeInputState(interactableRaycaster);
			_placementModeUIState = placementModeUIState.ThrowIfNull(nameof(placementModeUIState));

			_placementModeInputState.PlacementRequested += OnPlacementRequested;
			_placementModeInputState.PreviewPositionUpdated += OnPreviewPositionUpdated;
			_placementModeInputState.PreviewVisibilityUpdated += OnPreviewVisibilityUpdated;
			
			_placementModeUIState.HotBarItemSelected += OnHotBarItemSelected;
		}

		public WorldObjectDetails LatestPreviewWorldObject { get; set; }
			
		public override bool AllowCameraInputs => true;
		public override bool AllowInteractions => true;

		protected override IInputState InputState => _placementModeInputState;
		protected override UIState UIState => _placementModeUIState;

		protected override void OnEnabled()
		{
			RequestPreviewUpdate();
		}

		protected override void OnDisabled()
		{
			GameEvents.GameWorld.DeletePreviewWorldObject.Request(this);
		}

		private void OnPreviewPositionUpdated(object sender, WorldPositionEventArgs args)
		{
			_latestPreviewChunkPosition = args.ChunkPosition;
			_latestPreviewTilePosition = args.TilePosition;
			RequestPreviewUpdate();
		}

		private void OnPreviewVisibilityUpdated(object sender, bool visibility)
		{
			_latestPreviewVisibility = visibility;
			RequestPreviewUpdate();
		}
		
		private void OnHotBarItemSelected(object sender, WorldObjectDetails details)
		{
			LatestPreviewWorldObject = details;
			RequestPreviewUpdate();
		}
		
		private void OnPlacementRequested(object sender, WorldPositionEventArgs args)
		{
			GameEvents.GameWorld.PlaceWorldObject.Request(this, new PlacementEventArgs
			{
				ChunkPosition = args.ChunkPosition,
				TilePosition = args.TilePosition,
				WorldObjectDetails = LatestPreviewWorldObject
			});
		}

		private void RequestPreviewUpdate()
		{
			GameEvents.GameWorld.PlacePreviewWorldObject.Request(this, new PlacementEventArgs
			{
				WorldObjectDetails = LatestPreviewWorldObject,
				ChunkPosition = _latestPreviewChunkPosition,
				TilePosition = _latestPreviewTilePosition,
				Visibility = _latestPreviewVisibility
			});
		}
	}
}