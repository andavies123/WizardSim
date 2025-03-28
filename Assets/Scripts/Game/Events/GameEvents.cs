using UnityEngine;

namespace Game.Events
{
	public static class GameEvents
	{
		public static GameStateEvents GameState { get; private set; } = new();
		public static GameWorldEvents GameWorld { get; private set; } = new();
		public static GeneralEvents General { get; private set; } = new();
		public static InteractionEvents Interaction { get; private set; } = new();
		public static SettlementEvents Settlement { get; private set; } = new();
		public static UIEvents UI { get; private set; } = new();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			GameState = new GameStateEvents();
			GameWorld = new GameWorldEvents();
			General = new GeneralEvents();
			Interaction = new InteractionEvents();
			Settlement = new SettlementEvents();
			UI = new UIEvents();
		}
	}
}