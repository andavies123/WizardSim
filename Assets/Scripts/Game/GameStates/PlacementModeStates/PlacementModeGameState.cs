using System;
using CameraComponents;
using Extensions;
using Game.MessengerSystem;
using GameWorld.GameWorldEventArgs;
using GameWorld.Messages;
using UnityEngine;

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
			_placementModeUIState = placementModeUIState.ThrowIfNull(nameof(placementModeUIState));
			_placementModeInputState = new PlacementModeInputState(interactableRaycaster);

			_placementModeInputState.PlacementRequested += OnPlacementRequested;
			_placementModeInputState.PreviewPositionUpdated += OnPreviewPositionUpdated;
			_placementModeInputState.HidePlacementPreviewRequested += OnHidePlacementPreviewRequested;
		}

		public GameObject PlacementPrefab { get; set; }
			
		public override bool AllowCameraInputs => true;
		public override bool AllowInteractions => true;

		protected override IInputState InputState => _placementModeInputState;
		protected override UIState UIState => _placementModeUIState;
		
		protected override void OnEnabled() { }
		protected override void OnDisabled() { }

		private void OnPlacementRequested(object sender, WorldPositionEventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectPreviewRequest(args.ChunkPosition, args.TilePosition, PlacementPrefab));

		private void OnPreviewPositionUpdated(object sender, WorldPositionEventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectPlacementRequest(args.ChunkPosition, args.TilePosition, PlacementPrefab));

		private static void OnHidePlacementPreviewRequested(object sender, EventArgs args) =>
			GlobalMessenger.Publish(new WorldObjectHidePreviewRequest());
	}
}