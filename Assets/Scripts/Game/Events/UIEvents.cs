using System;
using UnityEngine;
using Upgrades;

namespace Game.Events
{
	public class UIEvents
	{
		// Events
		public GameEvent<UpgradeSelectedEventArgs> UpgradeSelected { get; } = new();
		
		// Requests
		
		public GameRequest<OpenUIEventArgs> OpenUI { get; } = new();
		public GameRequest<CloseUIEventArgs> CloseUI { get; } = new();
		
		public GameRequest<StartInteractionEventArgs> StartInteraction { get; } = new();
		public GameRequest EndInteraction { get; } = new();
	}

	public class UpgradeSelectedEventArgs : GameEventArgs
	{
		public Upgrade SelectedUpgrade { get; set; }
	}

	public class StartInteractionEventArgs : GameEventArgs
	{
		public Action<MonoBehaviour> InteractionCallback { get; set; }
	}

	public class OpenUIEventArgs : GameEventArgs
	{
		public UIWindow Window { get; set; }
	}

	public class CloseUIEventArgs : GameEventArgs
	{
		public UIWindow Window { get; set; }
	}
		
	public enum UIWindow
	{
		TownHallWindow,
		UpgradeWindow
	}
}