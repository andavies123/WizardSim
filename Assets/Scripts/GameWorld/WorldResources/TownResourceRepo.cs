using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld.WorldResources
{
	public class TownResourceRepo
	{
		private const string LoadFolderPath = "Scriptable Objects/Town Resources";
		
		private readonly Dictionary<Guid, TownResource> _resourcesById = new();
		
		/// <summary>
		/// Tries to get a town resource from the internal repository if it exists
		/// </summary>
		/// <param name="resourceId">The unique ID of the town resource</param>
		/// <param name="resource">The <see cref="TownResource"/> object stored in the repository</param>
		/// <returns>True if the resource exists. False if the resource does not exist</returns>
		public bool TryGetResourceById(Guid resourceId, out TownResource resource)
		{
			return _resourcesById.TryGetValue(resourceId, out resource);
		}
		
		/// <summary>
		/// Loads all Town Resources in a specified folder.
		/// Should only be called once at startup
		/// </summary>
		public void LoadAllTownResources()
		{
			foreach (TownResource townResource in Resources.LoadAll<TownResource>(LoadFolderPath))
			{
				CacheResource(townResource);
			}
		}

		private void CacheResource(TownResource resource)
		{
			if (!_resourcesById.TryAdd(resource.Id, resource))
			{
				Debug.Log($"Unable to load {resource}");
			}
		}
	}
}