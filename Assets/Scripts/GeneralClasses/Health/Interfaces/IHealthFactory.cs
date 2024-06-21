namespace GeneralClasses.Health.Interfaces
{
	public interface IHealthFactory
	{
		/// <summary>
		/// Creates a new health object based on the values from a given <see cref="HealthProperties"/> object
		/// </summary>
		/// <param name="healthProperties"><see cref="HealthProperties"/> object that contains info to create a new Health class</param>
		/// <returns>The created health class</returns>
		IHealth CreateHealth(HealthProperties healthProperties);
	}
}