using System;
using GameWorld.WorldObjects;
using UI.HotBarUI;
using UnityEngine;
using Utilities.Attributes;

namespace Game.GameStates.PlacementModeStates
{
	public class PlacementModeUIState : UIState
	{
		[SerializeField, Required] private HotBar hotBar;

		public event EventHandler<WorldObjectDetails> HotBarItemSelected;

		protected override void OnStateEnabled()
		{
			hotBar.HotBarItemSelected += OnHotBarItemSelected;
			
			hotBar.Show();
		}

		protected override void OnStateDisabled()
		{
			hotBar.HotBarItemSelected -= OnHotBarItemSelected;
			
			hotBar.Hide();
		}

		private void OnHotBarItemSelected(object sender, WorldObjectDetails details) =>
			HotBarItemSelected?.Invoke(sender, details);
	}
}