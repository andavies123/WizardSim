using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine;
using Upgrades;
using Utilities.Attributes;

namespace UI.Upgrades
{
	public class UpgradeScreen : MonoBehaviour
	{
		private const int UPGRADE_COUNT_CHALLENGE_COMPLETE = 3;
		private const int UPGRADE_COUNT_CHALLENGE_INCOMPLETE = 2;
		
		[Header("UI")]
		[SerializeField, Required] private Canvas canvas;
		[SerializeField, Required] private UpgradeCard upgradeCardPrefab;
		[SerializeField, Required] private Transform upgradeCardContainer;
		
		[Header("Non-UI")]
		[SerializeField, Required] private UpgradeManager upgradeManager;
		[SerializeField, Required] private ChallengesManager challengesManager;
		
		private readonly Queue<UpgradeCard> _pooledUpgradeCards = new();
		private readonly List<UpgradeCard> _activeUpgradeCards = new();

		private bool _isActive;
		
		public void Activate()
		{
			int upgradesToDisplay = challengesManager.CurrentChallenge?.CompletionCriteria?.Invoke() ?? false
				? UPGRADE_COUNT_CHALLENGE_COMPLETE
				: UPGRADE_COUNT_CHALLENGE_INCOMPLETE;
			List<Upgrade> upgrades = upgradeManager.GetRandomUpgrades(upgradesToDisplay);
			
			foreach (Upgrade upgrade in upgrades)
			{
				UpgradeCard upgradeCard = GetUpgradeCard();
				upgradeCard.Initialize(upgrade);
				upgradeCard.Selected += OnUpgradeCardSelected;
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
				upgradeCard.gameObject.SetActive(false);
				upgradeCard.Selected -= OnUpgradeCardSelected;
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
			_pooledUpgradeCards.Enqueue(upgradeCard);
		}
		
		private void OnUpgradeCardSelected(object sender, EventArgs args)
		{
			if (sender is not UpgradeCard upgradeCard)
				return;
			
			upgradeCard.Upgrade.Apply?.Invoke();
			Deactivate();
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