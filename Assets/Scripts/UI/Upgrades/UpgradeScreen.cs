using System.Collections.Generic;
using UnityEngine;
using Upgrades;
using Utilities.Attributes;

namespace UI.Upgrades
{
	public class UpgradeScreen : MonoBehaviour
	{
		[SerializeField, Required] private Canvas canvas;
		[SerializeField, Required] private UpgradeManager upgradeManager;
		[SerializeField, Required] private UpgradeCard upgradeCardPrefab;
		[SerializeField, Required] private Transform upgradeCardContainer;
		
		private readonly Queue<UpgradeCard> _pooledUpgradeCards = new();
		private readonly List<UpgradeCard> _activeUpgradeCards = new();

		private bool _isActive;
		
		public void Activate()
		{
			List<Upgrade> upgrades = upgradeManager.GetRandomUpgrades(2);
			
			foreach (Upgrade upgrade in upgrades)
			{
				UpgradeCard upgradeCard = GetUpgradeCard();
				upgradeCard.Initialize(upgrade);
				_activeUpgradeCards.Add(upgradeCard);
				upgradeCard.gameObject.SetActive(true);
			}

			canvas.enabled = true;
			_isActive = true;
		}

		public void Deactivate()
		{
			canvas.enabled = false;

			foreach (UpgradeCard upgradeCard in _activeUpgradeCards)
			{
				ReleaseUpgradeCard(upgradeCard);
			}
			_activeUpgradeCards.Clear();
			
			_isActive = false;
		}

		private UpgradeCard GetUpgradeCard()
		{
			if (!_pooledUpgradeCards.TryDequeue(out UpgradeCard upgradeCard))
				upgradeCard = Instantiate(upgradeCardPrefab, upgradeCardContainer);
			
			return upgradeCard;
		}

		private void ReleaseUpgradeCard(UpgradeCard upgradeCard)
		{
			upgradeCard.gameObject.SetActive(false);
			_pooledUpgradeCards.Enqueue(upgradeCard);
		}

		private void Awake()
		{
			Deactivate();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.U))
			{
				if (_isActive)
					Deactivate();
				else
					Activate();
			}
		}
	}
}