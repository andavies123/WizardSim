using TMPro;
using UnityEngine;
using Upgrades;
using Utilities.Attributes;

namespace UI.Upgrades
{
	// Todo: Add support for an icon
	// Todo: Add support for a general background color
	public class UpgradeCard : MonoBehaviour
	{
		[SerializeField, Required] private TMP_Text titleText;
		[SerializeField, Required] private TMP_Text descriptionText;

		public void Initialize(Upgrade upgrade)
		{
			titleText.SetText(upgrade.Title);
			descriptionText.SetText(upgrade.Description);
		}
	}

	// Todo: Add support for calculating how many upgrades to show
}