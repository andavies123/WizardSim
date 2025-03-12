using System;
using System.Collections.Concurrent;
using GameWorld.WorldObjects;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Builders
{
	[DisallowMultipleComponent]
	public class WorldObjectPool : MonoBehaviour
	{
		[SerializeField, Required] private int maxInactiveStorage = 10;

		private readonly ConcurrentDictionary<WorldObjectDetails, PoolGroup> _worldObjectsByDetails = new();
		
		public bool TryGetWorldObject(WorldObjectDetails details, out WorldObject worldObject)
		{
			if (!details)
			{
				worldObject = null;
				return false;
			}

			if (_worldObjectsByDetails.TryGetValue(details, out PoolGroup worldObjects) &&
			    worldObjects.TryGet(out worldObject))
			{
				return true;
			}

			worldObject = Instantiate(details.Prefab);
			worldObject.gameObject.name = details.Name;
			return true;
		}
		
		public void ReleaseObject(WorldObject objectToRelease)
		{
			if (!objectToRelease) return;

			PoolGroup poolGroup = _worldObjectsByDetails.GetOrAdd(objectToRelease.Details, details =>
			{
				Transform container = new GameObject().transform;
				container.SetParent(transform);
				return new PoolGroup(container, $"{details.Group} - {details.Name}", maxInactiveStorage);
			});
 
			if (!poolGroup.TryAdd(objectToRelease))
				Destroy(objectToRelease.gameObject);
		}

		private class PoolGroup
		{
			private readonly ConcurrentQueue<WorldObject> _queue = new();
			private readonly Transform _container;
			private readonly string _containerText;
			private readonly int _maxStorage;

			public PoolGroup(Transform container, string containerText, int maxStorage)
			{
				_container = container ? container : throw new ArgumentNullException(nameof(container));
				_containerText = containerText ?? "World Objects";
				_maxStorage = maxStorage;
			}

			public bool TryGet(out WorldObject worldObject)
			{
				if (_queue.TryDequeue(out worldObject))
				{
					_container.name = $"{_containerText} - {_queue.Count} pooled";
					return true;
				}

				return false;
			}

			public bool TryAdd(WorldObject worldObject)
			{
				if (_queue.Count >= _maxStorage)
					return false;

				_queue.Enqueue(worldObject);
				worldObject.gameObject.SetActive(false);
				worldObject.transform.SetParent(_container);
				_container.name = $"{_containerText} - {_queue.Count} pooled";
				return true;
			}
		}
	}
}