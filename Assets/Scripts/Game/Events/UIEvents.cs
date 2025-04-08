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
		public UpgradeSelectedEventArgs(Upgrade selectedUpgrade) => SelectedUpgrade = selectedUpgrade;
		
		public Upgrade SelectedUpgrade { get; }
	}

	public class StartInteractionEventArgs : GameEventArgs
	{
		public StartInteractionEventArgs(Action<MonoBehaviour> interactionCallback) =>
			InteractionCallback = interactionCallback;
		
		public Action<MonoBehaviour> InteractionCallback { get; }
	}

	public class OpenUIEventArgs : GameEventArgs
	{
		public OpenUIEventArgs(UIWindow window) => Window = window;

		public UIWindow Window { get; }
	}

	public class CloseUIEventArgs : GameEventArgs
	{
		public CloseUIEventArgs(UIWindow window) => Window = window;
        
		public UIWindow Window { get; }
	}
		
	public enum UIWindow
	{
		TownHallWindow,
		UpgradeWindow
	}
}