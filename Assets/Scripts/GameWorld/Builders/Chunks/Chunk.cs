using System;
using System.Collections.Concurrent;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Builders.Chunks
{ 
	public class Chunk : MonoBehaviour
	{
		public static readonly Vector2Int ChunkSize = new(10, 10);

		public Transform WorldObjectContainer { get; private set; }
		public ChunkData ChunkData { get; private set; }
		public ChunkMesh ChunkMesh { get; set; }
		public ConcurrentDictionary<Guid, WorldObject> WorldObjects { get; } = new();
		
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
			WorldObjects.Clear();
		}

		public void AddWorldObject(WorldObject worldObject)
		{
			WorldObjects.TryAdd(worldObject.Id, worldObject);
			worldObject.transform.SetParent(WorldObjectContainer);
		}

		public bool TryRemoveWorldObject(WorldObject worldObject)
		{
			return WorldObjects.TryRemove(worldObject.Id, out _);
		}

		private void Awake()
		{
			WorldObjectContainer = new GameObject("World Object Container").transform;
			WorldObjectContainer.SetParent(transform);
		}

		private void Start()
		{
			ChunkMesh.GetComponent<ChunkMesh>();
		}
	}
}