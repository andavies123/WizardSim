using System;
using System.Collections.Generic;
using System.Linq;
using GameWorld;
using GameWorld.WorldObjects;
using UnityEngine;
using Utilities.Attributes;
using static UI.HotBarUI.HotBarItem;

namespace UI.HotBarUI
{
	public class HotBar : UIComponent
	{
		[SerializeField, Required] private World world;
        
		private readonly Dictionary<WorldObjectDetails, HotBarItem> _hotBarItems = new();

		public event EventHandler<WorldObjectDetails> HotBarItemSelected;

		protected override void Awake()
		{
			base.Awake();
			
			FindAllHotBarItems();
			InitializeHotBarItems();
		}

		protected override void Start()
		{
			base.Start();
			
			world.WorldObjectManager.WorldObjectAdded += OnWorldObjectAdded;
			world.WorldObjectManager.WorldObjectRemoved += OnWorldObjectRemoved;
		}

		protected override void OnDestroy()
		{
			base.Start();
			
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
			HotBarItemSelected?.Invoke(sender, args.WorldObjectDetails);
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