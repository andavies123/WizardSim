using System;
using CameraComponents;
using Extensions;
using Game.MessengerSystem;
using GameWorld.GameWorldEventArgs;
using GameWorld.Messages;
using GameWorld.WorldObjectPreviews.Messages;
using GameWorld.WorldObjects;

namespace Game.GameStates.PlacementModeStates
{
	public class PlacementModeGameState : GameState
	{
		private readonly PlacementModeInputState _placementModeInputState;
		private readonly PlacementModeUIState _placementModeUIState;
		
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

		public WorldObjectDetails PlacementDetails { get; set; }
			
		public override bool AllowCameraInputs => true;
		public override bool AllowInteractions => true;

		protected override IInputState InputState => _placementModeInputState;
		protected override UIState UIState => _placementModeUIState;

		protected override void OnEnabled()
		{
			GlobalMessenger.Publish(new WorldObjectPreviewSetDetailsMessage
			{
				Sender = this,
				Details = PlacementDetails
			});
		}

		protected override void OnDisabled() =>
			GlobalMessenger.Publish(new WorldObjectPreviewDeleteMessage
			{
				Sender = this
			});

		private void OnPreviewPositionUpdated(object sender, WorldPositionEventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectPreviewSetPositionMessage
			{
				Sender = this,
				ChunkPosition = args.ChunkPosition,
				TilePosition = args.TilePosition
			});

		private void OnPreviewVisibilityUpdated(object sender, bool visibility) =>
			GlobalMessenger.Publish(new WorldObjectPreviewSetVisibilityMessage
			{
				Sender = this,
				Visibility = visibility
			});
		
		private void OnHotBarItemSelected(object sender, WorldObjectDetails details)
		{
			PlacementDetails = details;
			
			GlobalMessenger.Publish(new WorldObjectPreviewSetDetailsMessage
			{
				Sender = this,
				Details = details
			});
		}

		private void OnPlacementRequested(object sender, WorldPositionEventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectPlacementRequest
			{
				Sender = this,
				ChunkPosition = args.ChunkPosition,
				TilePosition = args.TilePosition,
				WorldObjectDetails = PlacementDetails
			});
	}
}