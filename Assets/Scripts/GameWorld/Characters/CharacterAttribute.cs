using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameWorld.Characters
{
	public class CharacterAttribute : INotifyPropertyChanged
	{
		private const int MaxLevel = 10;
        
		private static readonly int[] AllExperienceToLevelUp =
		{
			100,  // 0 -> 1
			150,  // 1 -> 2
			250,  // 2 -> 3
			400,  // 3 -> 4
			650,  // 4 -> 5
			1000, // 5 -> 6
			1500, // 6 -> 7
			2250, // 7 -> 8
			3000, // 8 -> 9
			5000  // 9 -> 10
		};
		
		public event Action LeveledUp;
		public event PropertyChangedEventHandler PropertyChanged;

		private int _currentLevel;
		public int CurrentLevel
		{
			get => _currentLevel;
			set
			{
				if (SetField(ref _currentLevel, Math.Clamp(value, 0, MaxLevel)))
					LeveledUp?.Invoke();
			}
		}

		private int _currentExperience;
		public int CurrentExperience
		{
			get => _currentExperience;
			set
			{
				if (IsAtMaxLevel)
					return;
				
				if (value >= ExperienceToLevelUp)
				{
					SetField(ref _currentExperience, value - AllExperienceToLevelUp[CurrentLevel]);
					CurrentLevel++;
					ExperienceToLevelUp = IsAtMaxLevel ? 0 : AllExperienceToLevelUp[CurrentLevel];
				}
				else 
					SetField(ref _currentExperience, value);
			}
		}

		private int _experienceToLevelUp;
		public int ExperienceToLevelUp
		{
			get => _experienceToLevelUp;
			set => SetField(ref _experienceToLevelUp, value);
		}

		private bool IsAtMaxLevel => CurrentLevel == MaxLevel;

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) 
				return false;
			
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}
}