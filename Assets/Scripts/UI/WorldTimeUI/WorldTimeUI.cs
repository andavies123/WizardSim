using System.Collections;
using Extensions;
using GameWorld;
using TMPro;
using UnityEngine;

namespace UI.WorldTimeUI
{
	public class WorldTimeUI : MonoBehaviour
	{
		[SerializeField] private WorldTime worldTime;
		[SerializeField] private TMP_Text timeText;
		[SerializeField] private float updatesPerSecond;
		
		private float _waitTime = 0f;

		public bool UpdateText { get; set; } = true;

		private void Awake()
		{
			worldTime.ThrowIfNull(nameof(worldTime));
			timeText.ThrowIfNull(nameof(timeText));

			_waitTime = 1 / updatesPerSecond;
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
				timeText.SetText($"Day: {worldTime.CurrentDays} ({worldTime.CurrentHours:00}:{worldTime.CurrentMinutes:00})");
				yield return new WaitForSeconds(_waitTime);
			}
		}
	}
}