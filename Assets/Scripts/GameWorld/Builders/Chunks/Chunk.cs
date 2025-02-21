using System;
using UnityEngine;

namespace GameWorld.Builders.Chunks
{ 
	public class Chunk : MonoBehaviour
	{
		public const int ChunkSize = 10;
		
		public ChunkData ChunkData { get; private set; }
		public ChunkMesh ChunkMesh { get; set; }
		
		public void Initialize(ChunkData chunkData)
		{
			ChunkData = chunkData ?? throw new ArgumentNullException(nameof(chunkData));
			gameObject.name = $"Chunk: {ChunkData.Position}";
		}

		public void CleanUp()
		{
			ChunkData = null;
			gameObject.name = "Chunk: Not Initialized";
			gameObject.SetActive(false);
		}

		private void Start()
		{
			ChunkMesh.GetComponent<ChunkMesh>();
		}
	}
}