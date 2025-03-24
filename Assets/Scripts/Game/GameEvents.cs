using System;
using UnityEngine;

namespace Game
{
	public static class GameEvents
	{
		public static GeneralEvents General { get; private set; } = new();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			General = new GeneralEvents();
		}
	}

	public class GameEvent<T> where T : GameEventArgs
	{
		public event EventHandler<T> Raised;
		public event EventHandler Requested;

		public void Raise(T args) => Raised?.Invoke(args.Sender, args);
		public void Request(object requester) => Requested?.Invoke(requester, EventArgs.Empty);
	}

	public class GameEventArgs : EventArgs
	{
		public GameEventArgs(object sender) => Sender = sender;
		
		public object Sender { get; set; }
	}

	public class GeneralEvents
	{
		public GameEvent<GameEventArgs> PauseGame { get; } = new();
		public GameEvent<GameEventArgs> ResumeGame { get; } = new();
		public GameEvent<GameEventArgs> QuitGame { get; } = new();
		public GameEvent<GameEventArgs> SaveGame { get; } = new();
	}

	public class SettlementEvents
	{
		public GameEvent<GameEventArgs> WizardAdded { get; } = new();
		public GameEvent<GameEventArgs> WizardDied { get; } = new();
	}
}