using System;
using GameWorld.WorldObjects;
using UI.HotBarUI;
using UI.InfoWindows;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace Game.GameStates.GameplayStates
{
	public class GameplayUIState : UIState
	{
		[Header("UI Elements")]
		[SerializeField, Required] private Button pauseButton;
		[SerializeField, Required] private HotBar hotBar;
		[SerializeField, Required] private InfoWindow infoWindow;
		
		public event EventHandler PauseButtonPressed;
		public event EventHandler<WorldObjectDetails> HotBarItemSelected;

		public InfoWindow InfoWindow => infoWindow;

        protected override void OnStateEnabled()
		{
			pauseButton.onClick.AddListener(OnPauseButtonPressed);

			hotBar.HotBarItemSelected += OnHotBarItemSelected;
			hotBar.Show();
		}

		protected override void OnStateDisabled()
		{
			pauseButton.onClick.RemoveListener(OnPauseButtonPressed);
			
			hotBar.HotBarItemSelected -= OnHotBarItemSelected;
			hotBar.Hide();
		}

		private void OnPauseButtonPressed() => 
			PauseButtonPressed?.Invoke(this, EventArgs.Empty);
		
		private void OnHotBarItemSelected(object sender, WorldObjectDetails details) => 
			HotBarItemSelected?.Invoke(sender, details);
	}
}