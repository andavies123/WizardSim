using System.Timers;

namespace GeneralClasses.Timers.Interfaces
{
	/// <summary>
	/// Interface to describe a timer
	/// </summary>
	public interface ITimer
	{
		/// <summary>
		/// Event raised when the timer reaches the interval
		/// </summary>
		event ElapsedEventHandler Elapsed;
		
		/// <summary>
		/// The time at which this timer will "elapse"
		/// and the <see cref="Elapsed"/> event is raised
		/// </summary>
		double Interval { get; set; }
		
		/// <summary>
		/// True => Timer will continue to raise the <see cref="Elapsed"/> event
		/// False => Timer will only raise the <see cref="Elapsed"/> event once
		/// </summary>
        bool AutoReset { get; set; }
        
		/// <summary>
		/// Starts the timer
		/// </summary>
		void Start();

		/// <summary>
		/// Stops the timer
		/// </summary>
		void Stop();

		/// <summary>
		/// Stops the timer then starts the timer back from the beginning
		/// </summary>
		void Restart();
	}
}