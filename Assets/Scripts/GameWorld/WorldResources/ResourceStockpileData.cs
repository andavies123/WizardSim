using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameWorld.WorldResources
{
	public class ResourceStockpileData : INotifyPropertyChanged
	{
		private int _maxCapacity;
		private int _currentTotal;
		private int _availableSpace;
		
		/// <summary>
		/// Raised when a property is changed for this instance
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
			
		/// <summary>
		/// The max amount allowed this stockpile can hold of this resource.
		/// Should not be negative
		/// </summary>
		public int MaxCapacity
		{
			get => _maxCapacity;
			set => SetField(value, ref _maxCapacity);
		}

		/// <summary>
		/// The current running total of this resource. Should not exceed <see cref="MaxCapacity"/>
		/// </summary>
		public int CurrentTotal
		{
			get => _currentTotal;
			set => SetField(value, ref _currentTotal);
		}

		/// <summary>
		/// The current available storage that this stockpile can hold of this resource.
		/// (MaxCapacity - CurrentTotal)
		/// </summary>
		public int AvailableSpace
		{
			get => _availableSpace;
			private set => SetField(value, ref _availableSpace);
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			switch (propertyName)
			{
				case nameof(MaxCapacity):
				case nameof(CurrentTotal):
					AvailableSpace = MaxCapacity - CurrentTotal;
					break;
			}
				
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void SetField<T>(T value, ref T field, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(value, field))
				return;
				
			field = value;
			OnPropertyChanged(propertyName);
		}
	}
}