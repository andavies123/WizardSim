using System.Linq;
using UnityEngine;

namespace GameWorld.Builders.Chunks
{
	/// <summary>
	/// Utility class used to hold constants and help with 
	/// </summary>
	public static class ChunkMeshHelper
	{
		/// <summary>
		/// Contains the vertices for a 1x1 2D square along the x/z axis
		/// </summary>
		public static readonly Vector3[] Vertices =
		{
			new(0, 0, 0),
			new(1, 0, 0),
			new(1, 0, 1),
			new(0, 0, 1)
		};

		/// <summary>
		/// Contains the UV coordinates for a 1x1 2D square
		/// </summary>
		public static readonly Vector2[] Uvs =
		{
			new(0, 0),
			new(1, 0),
			new(1, 1),
			new(0, 1)
		};

		/// <summary>
		/// Contains the indices that create 2 triangles based on the points in <see cref="Vertices"/>
		/// </summary>
		public static readonly int[] TriangleIndices =
		{
			0, 2, 1, // First Triangle 
			0, 3, 2  // Second Triangle
		};

		/// <summary>
		/// Helper method to add an offset to the <see cref="Vertices"/> array
		/// </summary>
		/// <param name="offsetPos">The position to add to each of the vertex points</param>
		/// <returns>The offset-ed array of vertices</returns>
		public static Vector3[] GetOffsetVertices(Vector3Int offsetPos) =>
			Vertices.ToList().Select(v => v + offsetPos).ToArray();

		/// <summary>
		/// Helper method to add an offset to the <see cref="TriangleIndices"/> array
		/// </summary>
		/// <param name="offsetFace">The face number used to offset the indices</param>
		/// <returns>The offset-ed array of triangle indices</returns>
		public static int[] GetOffsetTriangleIndices(int offsetFace) =>
			TriangleIndices.ToList().Select(ti => offsetFace * 4 + ti).ToArray();
	}
}