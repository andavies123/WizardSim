using System;
using System.ComponentModel;
using Game.Common;
using Game.Events;
using Game.Values;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.WorldTimeUI
{
	public class WorldTimeUI : MonoBehaviour
	{
		[SerializeField, Required] private TMP_Text timeText;

		[Header("Time Buttons")]
		[SerializeField, Required] private Button pauseSpeedButton;
		[SerializeField, Required] private Button normalSpeedButton;
		[SerializeField, Required] private Button doubleSpeedButton;
		[SerializeField, Required] private Button quadrupleSpeedButton;
		[SerializeField, Required] private TMP_Text speedText;
		[SerializeField] private Color selectedColor;
		[SerializeField] private Color unselectedColor;

		private Button _selectedButton;
		private float _waitTime;

		private TimeScaleGroup _pausedSpeedGroup;
		private TimeScaleGroup _normalSpeedGroup;
		private TimeScaleGroup _doubleSpeedGroup;
		private TimeScaleGroup _quadrupleSpeedGroup;

		private static int Day => GameValues.Time.Day;
		private static int Hour => GameValues.Time.Hour;
		private static int Minute => GameValues.Time.Minute;
		private static GameSpeed GameSpeed => GameValues.Time.GameSpeed;

		private void Awake()
		{
			_pausedSpeedGroup = new TimeScaleGroup(pauseSpeedButton, GameSpeed.Paused, "0x");
			_normalSpeedGroup = new TimeScaleGroup(normalSpeedButton, GameSpeed.Regular, "1x");
			_doubleSpeedGroup = new TimeScaleGroup(doubleSpeedButton, GameSpeed.Double, "2x");
			_quadrupleSpeedGroup = new TimeScaleGroup(quadrupleSpeedButton, GameSpeed.Quadruple, "4x");

			GameValues.Time.PropertyChanged += OnTimeValueChanged;
		}

		private void Start()
		{
			pauseSpeedButton.onClick.AddListener(OnPauseButtonPressed);
			normalSpeedButton.onClick.AddListener(OnNormalSpeedButtonPressed);
			doubleSpeedButton.onClick.AddListener(OnDoubleSpeedButtonPressed);
			quadrupleSpeedButton.onClick.AddListener(OnQuadrupleSpeedButtonPressed);

			//OnNormalSpeedButtonPressed();
			UpdateTimeScaleUI();
		}

		private void OnDestroy()
		{
			StopAllCoroutines();

			pauseSpeedButton.onClick.RemoveListener(OnPauseButtonPressed);
			normalSpeedButton.onClick.RemoveListener(OnNormalSpeedButtonPressed);
			doubleSpeedButton.onClick.RemoveListener(OnDoubleSpeedButtonPressed);
			quadrupleSpeedButton.onClick.RemoveListener(OnQuadrupleSpeedButtonPressed);
			
			GameValues.Time.PropertyChanged -= OnTimeValueChanged;
		}

		private void UpdateTimeText()
		{
			timeText.SetText($"Day: {Day} ({Hour:00}:{Minute:00})");
		}

		private void OnTimeValueChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(TimeValues.GameSpeed):
					UpdateTimeScaleUI();
					break;
				case nameof(TimeValues.Day):
				case nameof(TimeValues.Hour):
				case nameof(TimeValues.Minute):
					UpdateTimeText();
					break;
			}
		}

		private void OnPauseButtonPressed() => SetCurrentTimeScaleGroup(_pausedSpeedGroup);
		private void OnNormalSpeedButtonPressed() => SetCurrentTimeScaleGroup(_normalSpeedGroup);
		private void OnDoubleSpeedButtonPressed() => SetCurrentTimeScaleGroup(_doubleSpeedGroup);
		private void OnQuadrupleSpeedButtonPressed() => SetCurrentTimeScaleGroup(_quadrupleSpeedGroup);

		private void SetCurrentTimeScaleGroup(TimeScaleGroup group)
		{
			GameEvents.Time.ChangeGameSpeed.Request(this, new GameSpeedEventArgs { GameSpeed = group.Speed });
		}

		private void UpdateTimeScaleUI()
		{
			if (_selectedButton)
				_selectedButton.image.color = unselectedColor;
			
			TimeScaleGroup timeScaleGroup = GameSpeed switch
			{
				GameSpeed.Paused => _pausedSpeedGroup,
				GameSpeed.Regular => _normalSpeedGroup,
				GameSpeed.Double => _doubleSpeedGroup,
				GameSpeed.Quadruple => _quadrupleSpeedGroup,
				_ => throw new ArgumentOutOfRangeException(nameof(GameSpeed), GameSpeed, null)
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
			public GameSpeed Speed { get; }

			public TimeScaleGroup(Button button, GameSpeed speed, string scaleText)
			{
				Button = button;
				Speed = speed;
				ScaleText = scaleText;
			}
		}
	}
}