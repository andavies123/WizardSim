using System.Collections.Generic;
using System.Linq;
using Game;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Builders
{
	public abstract class WorldObjectFactory : MonoBehaviour, IWorldObjectFactory
	{
		private Dictionary<string, WorldObjectDetails> _detailsByName;
		private List<WorldObjectDetails> _details;

		public abstract string BuilderType { get; }
		public abstract WorldObject CreateObject(Vector2Int chunkPosition, Vector2Int localChunkPosition, WorldObjectDetails details);

		public IReadOnlyList<WorldObjectDetails> Details => _details;
		public IReadOnlyDictionary<string, WorldObjectDetails> DetailsByName => _detailsByName;

		protected virtual void Awake()
		{
			LoadWorldObjectDetails();
		}
		
		protected WorldObjectDetails GetRandomDetails() => _detailsByName.Values.ToList()[Random.Range(0, _detailsByName.Count)];

		private void LoadWorldObjectDetails()
		{
			if (!Globals.WorldObjectDetailsMap.DetailsByGroup.TryGetValue(BuilderType, out Dictionary<string, WorldObjectDetails> groupDetails))
			{
				Debug.LogError($"World Object Details do not exist for the following group: {BuilderType}");
				return;
			}
			
			_detailsByName = groupDetails;
			_details = groupDetails.Values.ToList();
		}
	}
}