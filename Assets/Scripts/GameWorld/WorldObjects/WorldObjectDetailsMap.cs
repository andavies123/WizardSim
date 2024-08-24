using System.Collections.Generic;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	public class WorldObjectDetailsMap : MonoBehaviour
	{
		private const string RESOURCES_PATH = "Scriptable Objects/World Object Details";
        
		private readonly Dictionary<string, WorldObjectDetails> _detailsMap = new();

		public bool TryGetDetails(string worldObjectName, out WorldObjectDetails details)
		{
			if (!_detailsMap.TryGetValue(worldObjectName, out details))
			{
				Debug.LogWarning($"World Object Details do not exist for \"{worldObjectName}\"");
				return false;
			}

			return true;
		}
        
		private void Awake()
		{
			WorldObjectDetails[] loadedDetails = LoadWorldObjectResources();
			CreateWorldObjectMap(loadedDetails);
		}

		private void CreateWorldObjectMap(WorldObjectDetails[] loadedDetails)
		{
			if (loadedDetails == null || loadedDetails.Length == 0)
			{
				Debug.LogWarning("There were no world object details loaded at runtime");
				return;
			}
			
			foreach (WorldObjectDetails details in loadedDetails)
			{
				if (!_detailsMap.TryAdd(details.Name, details))
				{
					// The only reason this should fail is if there already exists an entry with the same key
					Debug.LogWarning($"Unable to map world object details {details.Name}. Possible duplicate.");
				}
			}
		}
		
		private static WorldObjectDetails[] LoadWorldObjectResources()
		{
			return Resources.LoadAll<WorldObjectDetails>(RESOURCES_PATH);
		}
	}
}