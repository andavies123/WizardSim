using System;
using System.Collections.Generic;
using System.Linq;
using GameWorld.WorldResources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Upgrades
{
	public class IncreaseResourcesUpgradeGroup : UpgradeGroup
	{
		private readonly TownResourceStockpile _resourceStockpile;

		private readonly List<TownResource> _townResources;
		
		public IncreaseResourcesUpgradeGroup(float selectionWeight, TownResourceRepo resourceRepo, TownResourceStockpile resourceStockpile)
			: base(selectionWeight)
		{
			if (resourceRepo == null) throw new ArgumentNullException(nameof(resourceRepo));
			_resourceStockpile = resourceStockpile ? resourceStockpile : throw new ArgumentNullException(nameof(resourceStockpile));

			_townResources = resourceRepo.ResourcesById.Values.ToList();
		}

		public override Upgrade GetUpgrade()
		{
			TownResource townResource = _townResources[Random.Range(0, _townResources.Count)];
			int increaseAmount = Random.Range(1, 6) * 25;

			return new Upgrade
			{
				Id = $"{nameof(IncreaseResourcesUpgradeGroup)}.{townResource.DisplayName}",
				Title = $"Increase {townResource.DisplayName}",
				Description = $"Increases {townResource.DisplayName} by {increaseAmount}",
				Apply = () => _resourceStockpile.AddResources(townResource, increaseAmount),
				DisplaySettings = new UpgradeDisplaySettings
				{
					BackgroundColor = Color.green,
					OutlineColor = Color.black
				}
			};
		}
	}
}