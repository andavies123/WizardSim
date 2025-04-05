using Game.Common;

namespace Game.Values
{
	public class TimeValues : BaseValues
	{
		private int _day;
		public int Day
		{
			get => _day;
			set => SetField(ref _day, value);
		}

		private int _hour;
		public int Hour
		{
			get => _hour;
			set => SetField(ref _hour, value);
		}

		private int _minute;
		public int Minute
		{
			get => _minute;
			set => SetField(ref _minute, value);
		}
		
		private float _second;
		public float Second
		{
			get => _second;
			set => SetField(ref _second, value);
		}

		private GameSpeed _gameSpeed;
		public GameSpeed GameSpeed
		{
			get => _gameSpeed;
			set => SetField(ref _gameSpeed, value);
		}
	}
}