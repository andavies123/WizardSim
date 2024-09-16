using System.Collections;
using AndysTools.GameWorldTimeManagement.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.WorldTimeUI
{
	public class WorldTimeUI : MonoBehaviour
	{
		[SerializeField, Required] private GameWorldTimeBehaviour worldTime;
		[SerializeField, Required] private TMP_Text timeText;
		[SerializeField] private float updatesPerSecond;

		[Header("Time Buttons")]
		[SerializeField, Required] private Button pauseButton;
		[SerializeField, Required] private Button normalSpeedButton;
		[SerializeField, Required] private Button doubleSpeedButton;
		[SerializeField, Required] private Button quadrupleSpeedButton;
		[SerializeField] private Color selectedColor;
		[SerializeField] private Color unselectedColor;

		private Button _selectedButton;
		private float _waitTime = 0f;

		public bool UpdateText { get; set; } = true;

		private void Awake()
		{
			_waitTime = 1.0f / updatesPerSecond;
		}

		private void Start()
		{
			pauseButton.onClick.AddListener(OnPauseButtonPressed);
			normalSpeedButton.onClick.AddListener(OnNormalSpeedButtonPressed);
			doubleSpeedButton.onClick.AddListener(OnDoubleSpeedButtonPressed);
			quadrupleSpeedButton.onClick.AddListener(OnQuadrupleSpeedButtonPressed);

			OnNormalSpeedButtonPressed();
			StartCoroutine(nameof(UpdateTimeText));
		}

		private void OnDestroy()
		{
			StopAllCoroutines();

			pauseButton.onClick.RemoveListener(OnPauseButtonPressed);
			normalSpeedButton.onClick.RemoveListener(OnNormalSpeedButtonPressed);
			doubleSpeedButton.onClick.RemoveListener(OnDoubleSpeedButtonPressed);
			quadrupleSpeedButton.onClick.RemoveListener(OnQuadrupleSpeedButtonPressed);
		}

		private IEnumerator UpdateTimeText()
		{
			while (UpdateText)
			{
				timeText.SetText($"Day: {worldTime.Days} ({worldTime.Hours:00}:{worldTime.Minutes:00})");
				yield return new WaitForSeconds(_waitTime);
			}
		}

		private void OnPauseButtonPressed() => OnButtonSelected(0f, pauseButton);
		private void OnNormalSpeedButtonPressed() => OnButtonSelected(1f, normalSpeedButton);
		private void OnDoubleSpeedButtonPressed() => OnButtonSelected(2f, doubleSpeedButton);
		private void OnQuadrupleSpeedButtonPressed() => OnButtonSelected(4f, quadrupleSpeedButton);

		private void OnButtonSelected(float timeMultiplier, Button selectedButton)
		{
			Time.timeScale = timeMultiplier;
			worldTime.TimeMultiplier = timeMultiplier;

			if (_selectedButton)
				_selectedButton.image.color = unselectedColor;
			_selectedButton = selectedButton;
			if (_selectedButton)
				_selectedButton.image.color = selectedColor;
		}
	}
}