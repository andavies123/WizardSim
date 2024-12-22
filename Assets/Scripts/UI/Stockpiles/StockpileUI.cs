using System;
using System.Collections.Generic;
using GameWorld.WorldResources;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.Stockpiles
{
	[SelectionBase]
	public class StockpileUI : MonoBehaviour, IPointerClickHandler
	{
		private enum UISize { Small, Large }
		
		[SerializeField, Required] private TownResourceStockpile stockpile;
		[SerializeField, Required] private TownResourceLabel resourceLabelPrefab;

		[Header("Small UI")]
		[SerializeField] private Vector2 smallSize;
		[SerializeField, Required] private Transform smallLabelContainer;
		[SerializeField, Required] private List<TownResource> townResourcesToDisplaySmall = new();

		[Header("Large UI")]
		[SerializeField] private Vector2 largeSize;
		[SerializeField, Required] private Transform largeLabelContainer;
		[SerializeField, Required] private List<TownResource> townResourcesToDisplayLarge = new();

		private HorizontalLayoutGroup _parentLayoutGroup;
		private RectTransform _rectTransform;
		private UISize _uiSize = UISize.Small;

		private void Awake()
		{
			_parentLayoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
		}
		
		private void Start()
		{
			_rectTransform = transform as RectTransform;
			
			BuildLabels(townResourcesToDisplaySmall, smallLabelContainer);
			BuildLabels(townResourcesToDisplayLarge, largeLabelContainer);
			
			UpdateUI();
			return;

			void BuildLabels(List<TownResource> townResources, Transform container)
			{
				townResources.ForEach(townResource =>
				{
					if (stockpile.ResourceStockpiles.TryGetValue(townResource, out ResourceStockpileData stockpileData))
					{
						TownResourceLabel label = Instantiate(resourceLabelPrefab, container);
						label.Initialize(townResource, stockpileData);
					}
				});
			}
		}
		
		public void OnPointerClick(PointerEventData eventData)
		{
			_uiSize = _uiSize switch
			{
				UISize.Small => UISize.Large,
				UISize.Large => UISize.Small,
				_ => throw new IndexOutOfRangeException(_uiSize.ToString())
			};
			
			UpdateUI();
		}

		private void UpdateUI()
		{
			UpdateUISize();
			DisplayCorrectLabels();
		}

		private void UpdateUISize()
		{
			Vector2 size = _uiSize switch
			{
				UISize.Small => smallSize,
				UISize.Large => largeSize,
				_ => throw new IndexOutOfRangeException(_uiSize.ToString())
			};
			
			_rectTransform.sizeDelta = size;
			if (_parentLayoutGroup)
			{
				_parentLayoutGroup.SetLayoutVertical();
			}
		}

		private void DisplayCorrectLabels()
		{
			smallLabelContainer.gameObject.SetActive(_uiSize == UISize.Small);
			largeLabelContainer.gameObject.SetActive(_uiSize == UISize.Large);
		}
	}
}