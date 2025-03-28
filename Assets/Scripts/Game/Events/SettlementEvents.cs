namespace Game.Events
{
	public class SettlementEvents
	{
		public GameEvent<GameEventArgs> WizardAdded { get; } = new();
		public GameEvent<GameEventArgs> WizardDied { get; } = new();
		public GameEvent<GameEventArgs> BuildingAdded { get; } = new();
		public GameEvent<GameEventArgs> BuildingDestroyed { get; } = new();
	}
}