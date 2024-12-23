using System;
using System.Collections;
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
		[SerializeField, Required] private RectTransform smallLabelContainer;
		[SerializeField, Required] private List<TownResource> townResourcesToDisplaySmall = new();

		[Header("Large UI")]
		[SerializeField, Required] private RectTransform largeLabelContainer;
		[SerializeField, Required] private List<TownResource> townResourcesToDisplayLarge = new();

		private HorizontalLayoutGroup _parentLayoutGroup;
		private RectTransform _rectTransform;
		private UISize _uiSize = UISize.Small;

		private void Awake()
		{
			_parentLayoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
		}
		
		private IEnumerator Start()
		{
			_rectTransform = transform as RectTransform;
			
			BuildLabels(townResourcesToDisplaySmall, smallLabelContainer);
			BuildLabels(townResourcesToDisplayLarge, largeLabelContainer);

			yield return new WaitForEndOfFrame();
			
			UpdateUI();
			yield break;

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
			_rectTransform.sizeDelta = _uiSize switch
			{
				UISize.Small => new Vector2(300, smallLabelContainer.sizeDelta.y),
				UISize.Large => new Vector2(300, largeLabelContainer.sizeDelta.y),
				_ => throw new ArgumentOutOfRangeException(_uiSize.ToString())
			};
			
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