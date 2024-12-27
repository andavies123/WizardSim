using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld
{
	public class WorldObjectManager : IWorldObjectManager
	{
		private readonly ConcurrentDictionary<WorldObjectDetails, HashSet<WorldObject>> _worldObjectsByName = new();
		private readonly Transform _worldObjectParent;

		public event EventHandler<WorldObjectManagerEventArgs> WorldObjectAdded;
		public event EventHandler<WorldObjectManagerEventArgs> WorldObjectRemoved;

		public WorldObjectManager(Transform worldObjectParent)
		{
			_worldObjectParent = worldObjectParent;
		}

		public void AddWorldObject(WorldObject worldObject)
		{
			if (IsAtMaxCapacity(worldObject.Details))
			{
				Debug.LogWarning($"Unable to add world object - {worldObject.Details.Name}. Max allowed objects already exist...");
				return;
			}
			
			HashSet<WorldObject> worldObjects = _worldObjectsByName.GetOrAdd(worldObject.Details, new HashSet<WorldObject>());
			
			if (!worldObjects.Add(worldObject))
			{
				Debug.LogWarning("Attempting to add a world object that is already being stored");
				return;
			}

			worldObject.transform.parent = _worldObjectParent;
			WorldObjectAdded?.Invoke(this, new WorldObjectManagerEventArgs
			{
				AddedWorldObject = worldObject,
				Details = worldObject.Details,
				Count = GetObjectCount(worldObject.Details)
			});
		}

		public void RemoveWorldObject(WorldObject worldObject)
		{
			if (!_worldObjectsByName.TryGetValue(worldObject.Details, out HashSet<WorldObject> worldObjects))
			{
				Debug.LogWarning($"There are no existing world objects by the name: {worldObject.Details.Name}");
				return;
			}

			if (!worldObjects.Remove(worldObject))
			{
				Debug.LogWarning($"Unable to remove {worldObject.Details.Name}. It might have already been removed..");
				return;
			}

			WorldObjectRemoved?.Invoke(this, new WorldObjectManagerEventArgs
			{
				RemovedWorldObject = worldObject,
				Details = worldObject.Details,
				Count = GetObjectCount(worldObject.Details)
			});
		}
		
		public int GetObjectCount(WorldObjectDetails details)
		{
			if (!_worldObjectsByName.TryGetValue(details, out HashSet<WorldObject> worldObjects))
				return 0;

			return worldObjects.Count;
		}

		public bool IsAtMaxCapacity(WorldObjectDetails details)
		{
			int maxObjectsAllowed = details.PlacementProperties.MaxObjectsAllowed;
			
			if (maxObjectsAllowed == -1)
				return false;

			if (maxObjectsAllowed == 0)
				return true;

			return GetObjectCount(details) >= maxObjectsAllowed;
		}
	}
}