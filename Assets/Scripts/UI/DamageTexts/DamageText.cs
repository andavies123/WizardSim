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

		private Transform _transform;

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
		}

		public void UpdateWithTime(float t)
		{
			UpdatePosition();
			UpdateScale(t);
		}

		private void UpdatePosition()
		{
			_transform.position += 0.25f * Time.deltaTime * Vector3.up;
		}

		private void UpdateScale(float t)
		{
			// Formula used:
			// y = a * -tan((pi/2b)x) + c
			// a -> controls the sharpness of the downwards angle
			// b -> controls the width (x intercept)
			// c -> controls the height (y intercept)
			// y -> scale
			// x -> time
			
			const float sharpness = 0.2f;
			float scale = Mathf.Max(sharpness * -Mathf.Tan((Mathf.PI * t)/2) + 1, 0);
			_transform.localScale = new Vector3(scale, scale, scale);
		}
	}
}