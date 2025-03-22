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

	public class GameEvent
	{
		public event EventHandler Activated;
		public event EventHandler Requested;

		public void Activate(object sender) => Activated?.Invoke(sender, EventArgs.Empty);
		public void Request(object sender) => Requested?.Invoke(sender, EventArgs.Empty);
	}

	public class GeneralEvents
	{
		public GameEvent PauseGame { get; } = new();
		public GameEvent ResumeGame { get; } = new();
		public GameEvent QuitGame { get; } = new();
		public GameEvent SaveGame { get; } = new();
	}
}