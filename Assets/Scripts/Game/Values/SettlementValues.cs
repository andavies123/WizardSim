using GameWorld.WorldObjects;

namespace Game.Values
{
	public class SettlementValues : BaseValues
	{
		private string _settlementName;
		public string SettlementName
		{
			get => _settlementName;
			set => SetField(ref _settlementName, value);
		}
		
		private TownHall _townHall;
		public TownHall TownHall
		{
			get => _townHall;
			set => SetField(ref _townHall, value);
		}
	}
}