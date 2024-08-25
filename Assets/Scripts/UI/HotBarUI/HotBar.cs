using System.Collections.Generic;
using System.Linq;
using Extensions;
using Game.Messages;
using Game.MessengerSystem;
using GameWorld;
using GameWorld.WorldObjects;
using UnityEngine;
using static UI.HotBarUI.HotBarItem;

namespace UI.HotBarUI
{
	public class HotBar : MonoBehaviour
	{
		[SerializeField] private World world;
        
		private readonly Dictionary<WorldObjectDetails, HotBarItem> _hotBarItems = new();

		private void Awake()
		{
			world.ThrowIfNull(nameof(world));
			
			FindAllHotBarItems();
			InitializeHotBarItems();
		}

		private void Start()
		{
			world.WorldObjectManager.WorldObjectAdded += OnWorldObjectAdded;
			world.WorldObjectManager.WorldObjectRemoved += OnWorldObjectRemoved;
		}

		private void OnDestroy()
		{
			CleanUpHotBarItems();
			
			world.WorldObjectManager.WorldObjectAdded -= OnWorldObjectAdded;
			world.WorldObjectManager.WorldObjectRemoved -= OnWorldObjectRemoved;
		}

		private void FindAllHotBarItems()
		{
			List<HotBarItem> hotBarItems = GetComponentsInChildren<HotBarItem>().ToList();
			hotBarItems.ForEach(hotBarItem => _hotBarItems.Add(hotBarItem.WorldObjectDetails, hotBarItem));
		}

		private void InitializeHotBarItems()
		{
			_hotBarItems.Values.ToList().ForEach(hotBarItem => hotBarItem.Selected += OnHotBarItemSelected);
		}

		private void CleanUpHotBarItems()
		{
			_hotBarItems.Values.ToList().ForEach(hotBarItem => hotBarItem.Selected -= OnHotBarItemSelected);
		}

		private void OnHotBarItemSelected(object sender, HotBarItemSelectedEventArgs args)
		{
			GlobalMessenger.Publish(new BeginPlacementModeRequest(this, args.WorldObjectWorldObjectDetails));
		}

		private void OnWorldObjectAdded(object sender, WorldObjectManagerEventArgs args)
		{
			if (_hotBarItems.TryGetValue(args.Details, out HotBarItem hotBarItem) && world.WorldObjectManager.IsAtMaxCapacity(args.Details))
			{
				hotBarItem.Disable();
			}
		}

		private void OnWorldObjectRemoved(object sender, WorldObjectManagerEventArgs args)
		{
			if (_hotBarItems.TryGetValue(args.Details, out HotBarItem hotBarItem) && !world.WorldObjectManager.IsAtMaxCapacity(args.Details))
			{
				hotBarItem.Enable();
			}
		}
	}
}