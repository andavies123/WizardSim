using System.Collections.Concurrent;
using GameWorld.WorldObjects;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Builders
{
	[DisallowMultipleComponent]
	public class WorldObjectPool : MonoBehaviour
	{
		[SerializeField, Required] private Transform inactiveObjectParent;
		[SerializeField, Required] private int maxInactiveStorage = 10;

		private readonly ConcurrentDictionary<WorldObjectDetails, ConcurrentQueue<WorldObject>> _worldObjectsByDetails = new();
		
		public bool TryGetWorldObject(WorldObjectDetails details, out WorldObject worldObject)
		{
			if (!details)
			{
				worldObject = null;
				return false;
			}

			if (!_worldObjectsByDetails.TryGetValue(details, out ConcurrentQueue<WorldObject> worldObjects) ||
			    !worldObjects.TryDequeue(out worldObject))
			{
				worldObject = Instantiate(details.Prefab).GetComponent<WorldObject>();
			}

			return true;
		}
		
		public void ReleaseObject(WorldObject objectToRelease)
		{
			if (!objectToRelease) return;

			ConcurrentQueue<WorldObject> worldObjects = _worldObjectsByDetails.GetOrAdd(
				objectToRelease.Details, new ConcurrentQueue<WorldObject>());

			if (worldObjects.Count < maxInactiveStorage)
			{
				worldObjects.Enqueue(objectToRelease);
				objectToRelease.transform.SetParent(inactiveObjectParent);
			}
			else
			{
				Destroy(objectToRelease.gameObject);
			}
		}
	}
}