using System;
using UnityEngine;

namespace Game.Events
{
	public class UIEvents
	{
		public GameRequest<OpenUIEventArgs> OpenUI { get; } = new();
		public GameRequest<StartInteractionEventArgs> StartInteraction { get; } = new();
		public GameRequest EndInteraction { get; } = new();
	}

	public class StartInteractionEventArgs : GameEventArgs
	{
		public Action<MonoBehaviour> InteractionCallback { get; set; }
	}

	public class OpenUIEventArgs : GameEventArgs
	{
		public UIWindow Window { get; set; }
	}
		
	public enum UIWindow
	{
		TownHallWindow
	}
}