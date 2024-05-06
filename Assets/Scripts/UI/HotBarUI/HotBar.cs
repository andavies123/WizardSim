using System;
using Game.Messages;
using Game.MessengerSystem;
using UnityEngine;

namespace UI.HotBarUI
{
	public class HotBar : MonoBehaviour
	{
		[Header("Hot Bar Items")]
		[SerializeField] private HotBarItem placeRockHotBarItem;

		[Header("Prefabs")]
		[SerializeField] private GameObject rockPrefab;

		private void Awake() => InitializeButtons();
		private void OnDestroy() => CleanUpButtons();

		private void InitializeButtons()
		{
			placeRockHotBarItem.ButtonClicked += OnPlaceRockButtonClicked;
		}

		private void CleanUpButtons()
		{
			placeRockHotBarItem.ButtonClicked -= OnPlaceRockButtonClicked;
		}

		private void OnPlaceRockButtonClicked(object sender, EventArgs args) => GlobalMessenger.Publish(new BeginPlacementModeRequest(rockPrefab));
	}
}