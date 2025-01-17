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
		private float _timeCreated;
		private float _timeToLive;

		private void Awake()
		{
			_transform = transform;
		}

		public void Init(DamageType damageType, float damageAmount, float timeToLive)
		{
			// Transform
			_transform.localScale = Vector3.one;
			
			// Text
			text.color = damageType.DamageTextColor;
			text.SetText(damageAmount.ToString(CultureInfo.InvariantCulture));
			
			// Other
			_timeCreated = Time.time;
			_timeToLive = timeToLive;
		}

		public void Update()
		{
			UpdatePosition();
			UpdateScale(Time.time - _timeCreated);
		}

		private void UpdatePosition()
		{
			_transform.position += 0.25f * Time.deltaTime * Vector3.up;
		}

		private void UpdateScale(float time)
		{
			// Formula used:
			// y = a * -tan((pi/2b)x) + c
			// a -> controls the sharpness of the downwards angle
			// b -> controls the width (x intercept)
			// c -> controls the height (y intercept)
			// y -> scale
			// x -> time
			
			const float sharpness = 0.2f;
			float scale = Mathf.Max(sharpness * -Mathf.Tan((Mathf.PI * time)/(2 * _timeToLive)) + 1, 0);
			_transform.localScale = new Vector3(scale, scale, scale);
		}
	}
}