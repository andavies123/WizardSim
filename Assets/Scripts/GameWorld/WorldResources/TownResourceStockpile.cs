using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.WorldResources
{
	[Serializable]
	public class TownResourceDefaults
	{
		public TownResource townResource;
		public int startingMaxCapacity;
	}
	
	public class TownResourceStockpile : MonoBehaviour
	{
		[SerializeField, Required] private List<TownResourceDefaults> startingResources; 
        
		private readonly Dictionary<TownResource, ResourceStockpileData> _resourceStockpiles = new();

		/// <summary>
		/// Contains all resource stockpiles which include the max/total count/available space/etc...
		/// </summary>
		public IReadOnlyDictionary<TownResource, ResourceStockpileData> ResourceStockpiles => _resourceStockpiles;

		/// <summary>
		/// Initializes a resource type for the town's stockpile.
		/// If a resource is not initialized into this stockpile, <see cref="AddResources"/> will
		/// not add any resources to the stock pile
		/// </summary>
		/// <param name="resource">The resource to initialize in this stockpile</param>
		/// <param name="maxStorage">The max storage this resource currently has</param>
		public void InitializeResource(TownResource resource, int maxStorage)
		{
			bool resourceAdded = _resourceStockpiles.TryAdd(resource, new ResourceStockpileData
			{
				CurrentTotal = 0,
				MaxCapacity = Mathf.Max(maxStorage, 0)
			});

			if (!resourceAdded)
			{
				Debug.LogWarning($"Unable to initialize {resource} in the town stockpile. It must already be added");
			}
		}

		/// <summary>
		/// Updates the current max capacity for a specific town resource
		/// </summary>
		/// <param name="resource">The resource to update the max capacity for</param>
		/// <param name="newMaxCapacity">The new max capacity value. Min clamp to zero if value is negative</param>
		public void UpdateMaxCapacity(TownResource resource, int newMaxCapacity)
		{
			if (_resourceStockpiles.TryGetValue(resource, out ResourceStockpileData stockpileData))
			{
				stockpileData.MaxCapacity = Mathf.Max(newMaxCapacity, 0);
			}
			else
			{
				Debug.LogWarning($"Unable to update max capacity of {resource}");
			}
		}

		/// <summary>
		/// Increases the current total of a specific resource if it exists in this stockpile.
		/// Will not be able to exceed the max allowed storage.
		/// Cannot be used to decrease resources. Use <see cref="RemoveResources"/>
		/// </summary>
		/// <param name="resource">The type of resource to increase</param>
		/// <param name="count">The amount of this resource to increase by. Anything below zero will become zero</param>
		/// <returns>
		/// The amount of this resource that was added to a stockpile.
		/// Minimum that would be returned is 0
		/// </returns>
		public int AddResources(TownResource resource, int count)
		{
			if (count <= 0)
			{
				return 0;
			}
			
			if (_resourceStockpiles.TryGetValue(resource, out ResourceStockpileData stockpileData))
			{
				lock (stockpileData)
				{
					if (stockpileData.AvailableSpace == 0)
					{
						return 0;
					}
					
					// Pre-calculate how much to add
					int amountAdded = Mathf.Min(count, stockpileData.AvailableSpace);
					stockpileData.CurrentTotal += amountAdded;
					return amountAdded;
				}
			}

			return 0;
		}

		/// <summary>
		/// Decreases the current total of a specific resource if it exists in this stockpile.
		/// Will not be able to remove more than the current total.
		/// Cannot be used to increase resources. Use <see cref="AddResources"/>
		/// </summary>
		/// <param name="resource">The type of resource to decrease</param>
		/// <param name="count">The amount of this resource to decrease by. Anything below zero will become zero</param>
		/// <returns></returns>
		public int RemoveResources(TownResource resource, int count)
		{
			if (count <= 0)
			{
				return 0;
			}
			
			if (_resourceStockpiles.TryGetValue(resource, out ResourceStockpileData stockpileData))
			{
				lock (stockpileData)
				{
					// Pre-calculate how much can be removed
					int amountRemoved = Mathf.Min(count, stockpileData.CurrentTotal);
					stockpileData.CurrentTotal -= amountRemoved;
					return amountRemoved;
				}
			}

			return 0;
		}
		
		private void Awake()
		{
			startingResources.ForEach(resource => InitializeResource(resource.townResource, resource.startingMaxCapacity));
		}
	}
}