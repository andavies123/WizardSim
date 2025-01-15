using System.Globalization;
using TMPro;
using UnityEngine;
using Utilities.Attributes;

namespace UI.DamageTexts
{
	[SelectionBase]
	public class DamageText : MonoBehaviour
	{
		[SerializeField, Required] private TMP_Text text;
		[SerializeField] private float lifeTimeSeconds = 5f;

		private float _timeCreated;

		public void Init(DamageType damageType, float damageAmount)
		{
			text.color = damageType.DamageTextColor;
			text.SetText(damageAmount.ToString(CultureInfo.InvariantCulture));
			_timeCreated = Time.time;
		}

		public void Update()
		{
			if (Time.time - _timeCreated > lifeTimeSeconds)
				Destroy(gameObject);
		}
	}
}