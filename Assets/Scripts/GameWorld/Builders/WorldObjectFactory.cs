using System.Collections.Generic;
using System.Linq;
using Game;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Builders
{
	public abstract class WorldObjectFactory : MonoBehaviour, IWorldObjectFactory
	{
		protected IDictionary<string, WorldObjectDetails> GroupDetails;
		
		public abstract string BuilderType { get; }
		public abstract WorldObject CreateObject(Vector2Int chunkPosition, Vector2Int localChunkPosition);

		protected virtual void Start()
		{
			LoadWorldObjectDetails();
		}
		
		protected WorldObjectDetails GetRandomDetails() => GroupDetails.Values.ToList()[Random.Range(0, GroupDetails.Count)];

		private void LoadWorldObjectDetails()
		{
			if (!Global.WorldObjectDetailsMap.DetailsByGroup.TryGetValue(BuilderType, out IDictionary<string, WorldObjectDetails> groupDetails))
			{
				Debug.LogError($"World Object Details do not exist for the following group: {BuilderType}");
				return;
			}

			GroupDetails = groupDetails;
		}
	}
}