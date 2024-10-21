namespace GameWorld
{
	public static class StartGameplayDetails
	{
		public static LoadOrCreate LoadOrCreate { get; private set; }
		public static string WorldName { get; private set; }
		public static string WorldSeed { get; private set; }

		public static void CreateWorld(string worldName, string worldSeed)
		{
			LoadOrCreate = LoadOrCreate.Create;
			WorldName = worldName;
			WorldSeed = worldSeed;
		}
	}

	public enum LoadOrCreate
	{
		Load,
		Create
	}
}