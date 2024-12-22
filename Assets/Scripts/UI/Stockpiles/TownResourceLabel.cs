using System.ComponentModel;
using GameWorld.WorldResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.Stockpiles
{
	public class TownResourceLabel : MonoBehaviour
	{
		[SerializeField, Required] private Image resourceImage;
		[SerializeField, Required] private TMP_Text capacityLabel;

		private TownResource _townResource;
		private ResourceStockpileData _stockpileData;

		private string CurrentTotal => _stockpileData.CurrentTotal == 0 ? "-" : _stockpileData.CurrentTotal.ToString();
		private string MaxCapacity => _stockpileData.MaxCapacity == 0 ? "-" : _stockpileData.MaxCapacity.ToString();
		private string ResourceName => _townResource == null ? string.Empty : _townResource.DisplayName;
		
		public void Initialize(TownResource townResource, ResourceStockpileData stockpileData)
		{
			_townResource = townResource;
			_stockpileData = stockpileData;

			if (_townResource != null)
			{
				gameObject.name = $"{_townResource.DisplayName} Resource Label";
				UpdateResourceImage();
			}

			if (_stockpileData != null)
			{
				_stockpileData.PropertyChanged += OnStockpilePropertyChanged;
				UpdateCapacityLabel();
			}
		}

		private void OnStockpilePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(ResourceStockpileData.CurrentTotal):
				case nameof(ResourceStockpileData.MaxCapacity):
					UpdateCapacityLabel();
					break;
			}
		}

		private void UpdateResourceImage()
		{
			resourceImage.sprite = _townResource.DisplayImage;
		}

		private void UpdateCapacityLabel()
		{
			capacityLabel.SetText($"{ResourceName} {CurrentTotal}/{MaxCapacity}");
		}
	}
}