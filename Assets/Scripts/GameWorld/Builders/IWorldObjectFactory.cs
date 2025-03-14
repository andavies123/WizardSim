using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Builders
{
	public interface IWorldObjectFactory
	{
		/// <summary>
		/// Type of world object that this builder builds
		/// </summary>
		string BuilderType { get; }

		/// <summary>
		/// Creates a world object of this builder's type. This method will not be in charge of setting the position of the world object
		/// </summary>
		/// <param name="chunkPosition">The position of the chunk using "Chunk coordinates"</param>
		/// <param name="localChunkPosition">The position inside of the chunk</param>
		/// <param name="details">The world details to create</param>
		/// <returns>The world object that was created</returns>
		WorldObject CreateObject(Vector2Int chunkPosition, Vector2Int localChunkPosition, WorldObjectDetails details);
	}
}