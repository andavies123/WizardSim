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
		[SerializeField, Required] private Button pauseSpeedButton;
		[SerializeField, Required] private Button normalSpeedButton;
		[SerializeField, Required] private Button doubleSpeedButton;
		[SerializeField, Required] private Button quadrupleSpeedButton;
		[SerializeField, Required] private TMP_Text speedText;
		[SerializeField] private Color selectedColor;
		[SerializeField] private Color unselectedColor;

		private Button _selectedButton;
		private float _waitTime = 0f;

		private TimeScaleGroup _pausedSpeedGroup;
		private TimeScaleGroup _normalSpeedGroup;
		private TimeScaleGroup _doubleSpeedGroup;
		private TimeScaleGroup _quadrupleSpeedGroup;

		public bool UpdateText { get; set; } = true;

		private void Awake()
		{
			_waitTime = 1.0f / updatesPerSecond;

			_pausedSpeedGroup = new TimeScaleGroup(pauseSpeedButton, 0f, "0x");
			_normalSpeedGroup = new TimeScaleGroup(normalSpeedButton, 1f, "1x");
			_doubleSpeedGroup = new TimeScaleGroup(doubleSpeedButton, 2f, "2x");
			_quadrupleSpeedGroup = new TimeScaleGroup(quadrupleSpeedButton, 4f, "4x");
		}

		private void Start()
		{
			pauseSpeedButton.onClick.AddListener(OnPauseButtonPressed);
			normalSpeedButton.onClick.AddListener(OnNormalSpeedButtonPressed);
			doubleSpeedButton.onClick.AddListener(OnDoubleSpeedButtonPressed);
			quadrupleSpeedButton.onClick.AddListener(OnQuadrupleSpeedButtonPressed);

			OnNormalSpeedButtonPressed();
			StartCoroutine(nameof(UpdateTimeText));
		}

		private void OnDestroy()
		{
			StopAllCoroutines();

			pauseSpeedButton.onClick.RemoveListener(OnPauseButtonPressed);
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

		private void OnPauseButtonPressed() => SetCurrentTimeScaleGroup(_pausedSpeedGroup);
		private void OnNormalSpeedButtonPressed() => SetCurrentTimeScaleGroup(_normalSpeedGroup);
		private void OnDoubleSpeedButtonPressed() => SetCurrentTimeScaleGroup(_doubleSpeedGroup);
		private void OnQuadrupleSpeedButtonPressed() => SetCurrentTimeScaleGroup(_quadrupleSpeedGroup);

		private void SetCurrentTimeScaleGroup(TimeScaleGroup group)
		{
			Time.timeScale = group.Scale;
			worldTime.TimeMultiplier = group.Scale;
			speedText.SetText(group.ScaleText);

			if (_selectedButton)
				_selectedButton.image.color = unselectedColor;
			_selectedButton = group.Button;
			if (_selectedButton)
				_selectedButton.image.color = selectedColor;
		}

		private class TimeScaleGroup
		{
			public Button Button { get; }
			public float Scale { get; }
			public string ScaleText { get; }

			public TimeScaleGroup(Button button, float scale, string scaleText)
			{
				Button = button;
				Scale = scale;
				ScaleText = scaleText;
			}
		}
	}
}