using System.Collections.Generic;
using System.Linq;
using Game.Messages;
using Game.MessengerSystem;
using UnityEngine;
using static UI.HotBarUI.HotBarItem;

namespace UI.HotBarUI
{
	public class HotBar : MonoBehaviour
	{
		private List<HotBarItem> _hotBarItems = new();

		private void Awake()
		{
			FindAllHotBarItems();
			InitializeButtons();
		}

		private void OnDestroy() => CleanUpButtons();

		private void FindAllHotBarItems() => _hotBarItems = GetComponentsInChildren<HotBarItem>().ToList();
		private void InitializeButtons() => _hotBarItems.ForEach(hotBarItem => hotBarItem.Selected += OnHotBarItemSelected);
		private void CleanUpButtons() => _hotBarItems.ForEach(hotBarItem => hotBarItem.Selected -= OnHotBarItemSelected);

		private void OnHotBarItemSelected(object sender, HotBarItemSelectedEventArgs args) =>
			GlobalMessenger.Publish(new BeginPlacementModeRequest(this, args.WorldObjectWorldObjectDetails));
	}
}