using System.Collections.Generic;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	public class WorldObjectDetailsMap : MonoBehaviour
	{
		private const string RESOURCES_PATH = "Scriptable Objects/World Object Details";
        
		private readonly Dictionary<string, Dictionary<string, WorldObjectDetails>> _detailsByGroup = new();
        
		public IReadOnlyDictionary<string, Dictionary<string, WorldObjectDetails>> DetailsByGroup => _detailsByGroup;
		
		public bool TryGetDetails(string detailsGroup, string detailsName, out WorldObjectDetails details)
		{
			details = null;
			
			if (!_detailsByGroup.TryGetValue(detailsGroup, out Dictionary<string, WorldObjectDetails> groupDetails))
				return false;

			return groupDetails.TryGetValue(detailsName, out details);
		}
        
		private void Awake()
		{
			CreateWorldObjectMap(LoadWorldObjectResources());
		}

		private void CreateWorldObjectMap(IReadOnlyCollection<WorldObjectDetails> loadedDetails)
		{
			if (loadedDetails == null || loadedDetails.Count == 0)
			{
				Debug.LogWarning("There were no world object details loaded at runtime");
				return;
			}
			
			foreach (WorldObjectDetails details in loadedDetails)
			{
				if (_detailsByGroup.TryGetValue(details.Group, out Dictionary<string, WorldObjectDetails> groupDetails))
				{
					if (!groupDetails.TryAdd(details.Name, details))
					{
						Debug.LogWarning($"Possible world object details duplication | Problem object: \"{details.name}\" | Existing object: \"{groupDetails[details.Name].name}\"", details);
					}
				}
				else
				{
					if (!_detailsByGroup.TryAdd(details.Group, new Dictionary<string, WorldObjectDetails> { {details.Name, details} }))
					{
						// This should probably never happen
						Debug.LogWarning($"Unable to add new world object details group: {details.Group}");
					}
				}
			}
		}
		
		private static WorldObjectDetails[] LoadWorldObjectResources()
		{
			return Resources.LoadAll<WorldObjectDetails>(RESOURCES_PATH);
		}
	}
}