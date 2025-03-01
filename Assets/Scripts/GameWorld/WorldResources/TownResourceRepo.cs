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
		/// Collection of all loaded town resources stored by their unique resource Id
		/// </summary>
		public IReadOnlyDictionary<Guid, TownResource> ResourcesById => _resourcesById;
		
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