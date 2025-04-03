using System;
using System.Collections;
using AndysTools.GameWorldTimeManagement.Runtime;
using Game.Events;
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

			_pausedSpeedGroup = new TimeScaleGroup(pauseSpeedButton, GameSpeed.Paused, "0x");
			_normalSpeedGroup = new TimeScaleGroup(normalSpeedButton, GameSpeed.Regular, "1x");
			_doubleSpeedGroup = new TimeScaleGroup(doubleSpeedButton, GameSpeed.Double, "2x");
			_quadrupleSpeedGroup = new TimeScaleGroup(quadrupleSpeedButton, GameSpeed.Quadruple, "4x");

			GameEvents.TimeEvents.GameSpeedChanged.Raised += OnGameSpeedChanged;
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
			
			GameEvents.TimeEvents.GameSpeedChanged.Raised -= OnGameSpeedChanged;
		}

		private IEnumerator UpdateTimeText()
		{
			while (UpdateText)
			{
				timeText.SetText($"Day: {worldTime.Days} ({worldTime.Hours:00}:{worldTime.Minutes:00})");
				yield return new WaitForSeconds(_waitTime);
			}
		}

		private void OnGameSpeedChanged(object sender, GameSpeedEventArgs args)
		{
			UpdateTimeScaleUI(args.GameSpeed);
		}

		private void OnPauseButtonPressed() => SetCurrentTimeScaleGroup(_pausedSpeedGroup);
		private void OnNormalSpeedButtonPressed() => SetCurrentTimeScaleGroup(_normalSpeedGroup);
		private void OnDoubleSpeedButtonPressed() => SetCurrentTimeScaleGroup(_doubleSpeedGroup);
		private void OnQuadrupleSpeedButtonPressed() => SetCurrentTimeScaleGroup(_quadrupleSpeedGroup);

		private void SetCurrentTimeScaleGroup(TimeScaleGroup group)
		{
			GameEvents.TimeEvents.ChangeGameSpeed.Request(this, new GameSpeedEventArgs { GameSpeed = group.GameSpeed });
		}

		private void UpdateTimeScaleUI(GameSpeed gameSpeed)
		{
			if (_selectedButton)
				_selectedButton.image.color = unselectedColor;
			
			TimeScaleGroup timeScaleGroup = gameSpeed switch
			{
				GameSpeed.Paused => _pausedSpeedGroup,
				GameSpeed.Regular => _normalSpeedGroup,
				GameSpeed.Double => _doubleSpeedGroup,
				GameSpeed.Quadruple => _quadrupleSpeedGroup,
				_ => throw new ArgumentOutOfRangeException(nameof(gameSpeed), gameSpeed, null)
			};
			_selectedButton = timeScaleGroup.Button;
			
			if (_selectedButton)
				_selectedButton.image.color = selectedColor;
			
			speedText.SetText(timeScaleGroup.ScaleText);
		}

		private class TimeScaleGroup
		{
			public Button Button { get; }
			public string ScaleText { get; }
			public GameSpeed GameSpeed { get; }

			public TimeScaleGroup(Button button, GameSpeed gameSpeed, string scaleText)
			{
				Button = button;
				GameSpeed = gameSpeed;
				ScaleText = scaleText;
			}
		}
	}
}