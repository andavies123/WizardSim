using GameWorld.WorldObjects;

namespace Game.Events
{
	public class SettlementEvents
	{
		// Wizards
		public GameEvent WizardAdded { get; } = new();
		public GameEvent WizardDied { get; } = new();
		
		// Buildings
		public GameEvent BuildingAdded { get; } = new();
		public GameEvent BuildingDestroyed { get; } = new();
		public GameEvent<TownHallPlacedEventArgs> TownHallPlaced { get; } = new();
	}

	public class TownHallPlacedEventArgs : GameEventArgs
	{
		public TownHallPlacedEventArgs(TownHall townHall) => TownHall = townHall;
		
		public TownHall TownHall { get; }
	}
}