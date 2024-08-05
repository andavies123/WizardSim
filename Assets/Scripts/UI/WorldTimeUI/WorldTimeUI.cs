using System.Collections;
using AndysTools.GameWorldTimeManagement;
using AndysTools.GameWorldTimeManagement.Runtime;
using Extensions;
using TMPro;
using UnityEngine;

namespace UI.WorldTimeUI
{
	public class WorldTimeUI : MonoBehaviour
	{
		[SerializeField] private GameWorldTimeBehaviour worldTime;
		[SerializeField] private TMP_Text timeText;
		[SerializeField] private float updatesPerSecond;
		
		private float _waitTime = 0f;

		public bool UpdateText { get; set; } = true;

		private void Awake()
		{
			worldTime.ThrowIfNull(nameof(worldTime));
			timeText.ThrowIfNull(nameof(timeText));

			_waitTime = 1.0f / updatesPerSecond;
		}

		private void Start()
		{
			StartCoroutine(nameof(UpdateTimeText));
		}

		private void OnDestroy()
		{
			StopAllCoroutines();
		}

		private IEnumerator UpdateTimeText()
		{
			while (UpdateText)
			{
				timeText.SetText($"Day: {worldTime.Days} ({worldTime.Hours:00}:{worldTime.Minutes:00})");
				yield return new WaitForSeconds(_waitTime);
			}
		}
	}
}