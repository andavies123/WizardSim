using System;
using GeneralClasses.Health.HealthEventArgs;

namespace GeneralClasses.Health.Interfaces
{
	/// <summary>
	/// Interface to describe a Health object
	/// </summary>
	public interface IHealth
	{
		/// <summary>
		/// Raised when the value of <see cref="CurrentHealth"/> reaches <see cref="MaxHealth"/>
		/// </summary>
		public event EventHandler<ReachedMaxHealthEventArgs> ReachedMaxHealth;
		
		/// <summary>
		/// Raised when the value of <see cref="CurrentHealth"/> reaches zero
		/// </summary>
		public event EventHandler<ReachedMinHealthEventArgs> ReachedMinHealth;
		
		/// <summary>
		/// Raised when the value of <see cref="CurrentHealth"/> changes
		/// </summary>
		public event EventHandler<CurrentHealthChangedEventArgs> CurrentHealthChanged;
		
		/// <summary>
		/// Raised when the value of <see cref="MaxHealth"/> changes
		/// </summary>
		public event EventHandler<MaxHealthChangedEventArgs> MaxHealthChanged;
		
		/// <summary>
		/// Returns true when <see cref="CurrentHealth"/> is <see cref="MaxHealth"/>
		/// Returns false when <see cref="CurrentHealth"/> is less than <see cref="MaxHealth"/>
		/// </summary>
		public bool IsAtMaxHealth { get; }
		
		/// <summary>
		/// Returns true when <see cref="CurrentHealth"/> is zero
		/// Returns false when <see cref="CurrentHealth"/> is greater than zero
		/// </summary>
		public bool IsAtMinHealth { get; }
		
		/// <summary>
		/// The max value the <see cref="CurrentHealth"/> gets clamped at
		/// </summary>
		public float MaxHealth { get; set; }
		
		/// <summary>
		/// The current health value. Clamped between <see cref="MaxHealth"/> and zero
		/// </summary>
		public float CurrentHealth { get; set; }
	}
}