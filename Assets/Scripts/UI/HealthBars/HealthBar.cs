using Extensions;
using GameObjectPools;
using GeneralClasses.Health.HealthEventArgs;
using GeneralClasses.Health.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.HealthBars
{
	[RequireComponent(typeof(Canvas))]
	public class HealthBar : MonoBehaviour, IGameObjectPoolItem
	{
		private static readonly Vector3 DefaultPositionOffset = new(0, 1.5f, 0);

		[SerializeField, Required] private Image fillImage;
		[SerializeField, Required] private Gradient colorGradient;

		private Canvas _canvas;
		private Transform _transform;
		private bool _requiresUpdate = false;

		// Health object variables
		private IHealth _health;
		private Transform _followTransform;
		private Vector3 _positionOffset;

		// Fade to Destroy variables
		private bool _startFading = false;
		private bool _isFading = false;
		private float _timeToFadeSeconds = 0f;

		public event EventHandler ReleaseRequested;
        
		public void SetHealth(IHealth health, Transform followTransform)
		{
			// Check if the health object is the same
			if (_health == health)
				return;
			
			// Clean up old health
			if (_health != null)
				_health.CurrentHealthChanged -= OnHealthChanged;
			
			// Set new health
			_health = health;
			_followTransform = followTransform;

			if (_health == null)
				return;

			if (_health is IHealthBarUser healthBarUser)
				_positionOffset = healthBarUser.PositionOffset;
			else
				_positionOffset = DefaultPositionOffset;

			_health.CurrentHealthChanged += OnHealthChanged;
			UpdatePosition(); // Update the position
			_requiresUpdate = true; // Initialize the UI
			_canvas.enabled = _health != null; // Update the canvas
		}

		public void BeginFading(float timeToFadeSeconds)
		{
			if (_startFading || _isFading)
				return;

			_startFading = true;
			_timeToFadeSeconds = timeToFadeSeconds;
		}

		public void Initialize()
		{
			_canvas.enabled = false; // Make sure the health bar can't be seen until necessary
		}

		public void CleanUp()
		{
			if (_health != null)
			{
				_health.CurrentHealthChanged -= OnHealthChanged;
				_health = null;
			}

			_followTransform = null;
			_requiresUpdate = false;
			_canvas.enabled = false;
			
			// Reset fading values
			_startFading = false;
			_isFading = false;
		}

		private IEnumerator FadeOut()
		{
			_isFading = true;
			List<Image> imagesToFade = GetComponentsInChildren<Image>().ToList();
			List<Color> originalColors = new();

			foreach (Image image in imagesToFade)
			{
				originalColors.Add(image.color);
			}

			for (float t = 0f; t < _timeToFadeSeconds; t += Time.deltaTime)
			{
				for (int imageIndex = 0; imageIndex < imagesToFade.Count; imageIndex++)
				{
					imagesToFade[imageIndex].color = new Color(
						originalColors[imageIndex].r,
						originalColors[imageIndex].g,
						originalColors[imageIndex].b,
						Mathf.Lerp(originalColors[imageIndex].a, 0, Mathf.Min(1, t / _timeToFadeSeconds)));
				}

				yield return null;
			}

			ReleaseRequested?.Invoke(this, EventArgs.Empty);
		}

		private void UpdatePosition()
		{
			if (!_followTransform)
				return;

			_transform.position = _followTransform.position + _positionOffset;
		}

		private void UpdateImageFill(float currentHealth, float maxHealth)
		{
			float healthPercentage = currentHealth.PercentageOf01(maxHealth);
			fillImage.fillAmount = healthPercentage;
			fillImage.color = colorGradient.Evaluate(healthPercentage);
		}

		private void OnHealthChanged(object sender, CurrentHealthChangedEventArgs args)
		{
			_requiresUpdate = true;
		}

		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			_transform = transform;
			_canvas.enabled = false;
		}

		private void Update()
		{
			if (_startFading)
			{
				_startFading = false;
				StartCoroutine(FadeOut());
				return;
			}

			if (_health == null)
				return;

			// This is a check to see if the follow object still exists or not
			if (!_followTransform)
			{
				ReleaseRequested?.Invoke(this, EventArgs.Empty);
				return;
			}

			// Update position to follow the health user
			UpdatePosition();

			// Update the image if an update is required.
			// This is used instead of updating when the value changes is due to the
			// fact that UI can't be updated on a backup thread such as an elapsed timer.
			if (_requiresUpdate)
			{
				UpdateImageFill(_health.CurrentHealth, _health.MaxHealth);
				_requiresUpdate = false;
			}
		}
	}
}