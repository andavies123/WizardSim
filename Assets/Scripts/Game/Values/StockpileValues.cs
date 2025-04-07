using System;

namespace Game.Values
{
	public class StockpileValues : BaseValues
	{
		private int _stone;
		public int Stone
		{
			get => _stone;
			set => SetField(ref _stone, Math.Clamp(value, 0, StoneMax));
		}

		private int _stoneMax;
		public int StoneMax
		{
			get => _stoneMax;
			set => SetField(ref _stoneMax, Math.Max(value, 0));
		}

		private int _wood;
		public int Wood
		{
			get => _wood;
			set => SetField(ref _wood, Math.Clamp(value, 0, WoodMax));
		}

		private int _woodMax;
		public int WoodMax
		{
			get => _woodMax;
			set => SetField(ref _woodMax, Math.Max(value, 0));
		}
	}
}