using System;

namespace Game.Values
{
	public class SettingsValues : BaseValues
	{
		public static int MaxVolume => 10;
		
		private int _musicVolume = 5;
		public int MusicVolume
		{
			get => _musicVolume;
			set => SetField(ref _musicVolume, Math.Clamp(value, 0, MaxVolume));
		}

		private int _gameVolume = 5;
		public int GameVolume
		{
			get => _gameVolume;
			set => SetField(ref _gameVolume, Math.Clamp(value, 0, MaxVolume));
		}
	}
}