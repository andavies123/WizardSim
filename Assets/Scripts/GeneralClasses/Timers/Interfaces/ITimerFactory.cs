namespace GeneralClasses.Timers.Interfaces
{
	/// <summary>
	/// Interface to describe a factory that creates ITimer objects
	/// </summary>
	public interface ITimerFactory
	{
		/// <summary>
		/// Creates and returns a new timer object
		/// </summary>
		/// <param name="interval">The interval of the timer in milliseconds</param>
		/// <param name="autoReset">True if the timer will continue to raise the elapsed event</param>
		/// <returns>New <see cref="ITimer"/> object</returns>
		ITimer Create(double interval, bool autoReset);
	}
}