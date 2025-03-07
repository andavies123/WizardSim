using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Upgrades;
using Utilities.Attributes;

namespace UI.Upgrades
{
	// Todo: Add support for an icon
	// Todo: Add support for an outline color
	public class UpgradeCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[SerializeField, Required] private TMP_Text titleText;
		[SerializeField, Required] private TMP_Text descriptionText;
		[SerializeField, Required] private Image backgroundImage;

		private readonly Color _tintColor = new(0.75f, 0.75f, 0.75f);

		private bool _isHovered;

		public event EventHandler Selected;
		
		public Upgrade Upgrade { get; private set; }

		public void Initialize(Upgrade upgrade)
		{
			Upgrade = upgrade;
			titleText.SetText(upgrade.Title);
			descriptionText.SetText(upgrade.Description);
			backgroundImage.color = upgrade.DisplaySettings.BackgroundColor;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Selected?.Invoke(this, EventArgs.Empty);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_isHovered = true;
			UpdateVisuals();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_isHovered = false;
			UpdateVisuals();
		}

		private void UpdateVisuals()
		{
			if (_isHovered)
				backgroundImage.color = Upgrade.DisplaySettings.BackgroundColor * _tintColor;
			else
				backgroundImage.color = Upgrade.DisplaySettings.BackgroundColor;
		}
	}
}