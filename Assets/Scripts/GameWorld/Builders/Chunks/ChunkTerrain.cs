namespace GameWorld.Builders.Chunks
{
	public class ChunkTerrain
	{
		public readonly int[,] GrassType;

		public ChunkTerrain(int chunkSize)
		{
			GrassType = new int[chunkSize, chunkSize];
		}
	}
}