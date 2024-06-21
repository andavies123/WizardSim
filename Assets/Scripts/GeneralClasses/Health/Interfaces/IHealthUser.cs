namespace GeneralClasses.Health.Interfaces
{
	/// <summary>
	/// Interface to describe an object that should contain an <see cref="IHealth"/> object
	/// </summary>
	public interface IHealthUser
	{
		/// <summary>
		/// <see cref="IHealth"/> object reference
		/// </summary>
		public IHealth Health { get; }
	}
}