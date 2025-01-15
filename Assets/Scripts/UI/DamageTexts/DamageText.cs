using System.Globalization;
using Extensions;
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

		private Transform _transform;
		private float _timeCreated;

		private void Awake()
		{
			_transform = transform;
		}

		public void Init(DamageType damageType, float damageAmount)
		{
			// Transform
			_transform.localScale = Vector3.one;
			
			// Text
			text.color = damageType.DamageTextColor;
			text.SetText(damageAmount.ToString(CultureInfo.InvariantCulture));
			
			// Other
			_timeCreated = Time.time;
		}

		public void Update()
		{
			if (Time.time - _timeCreated > lifeTimeSeconds)
				Destroy(gameObject);

			_transform.position = _transform.position.SubY(_transform.position.y + 0.25f * Time.deltaTime);
			_transform.position += 0.15f * Time.deltaTime * Vector3.up;
			_transform.localScale -= 0.2f * Time.deltaTime * Vector3.one;
		}
	}
}