using Extensions;
using GameObjectPools;
using GeneralClasses.Health.HealthEventArgs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneralBehaviours.HealthBehaviours;
using GeneralComponents;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.HealthBars
{
	[RequireComponent(typeof(AlwaysFaceCamera))]
	[RequireComponent(typeof(Canvas))]
	public class HealthBar : MonoBehaviour, IGameObjectPoolItem
	{
		private static readonly Vector3 DefaultPositionOffset = new(0, 1.5f, 0);

		[SerializeField, Required] private Image fillImage;
		[SerializeField, Required] private Gradient colorGradient;

		private Canvas _canvas;
		private Transform _transform;
		private bool _requiresUpdate;
		private bool _enableCanvasFlag;

		// Health object variables
		private Transform _followTransform;
		private Vector3 _positionOffset;

		// Fade to Destroy variables
		private readonly List<Color> _originalColors = new();
		private List<Image> _imagesToFade;
		private bool _startFading;
		private bool _isFading;
		private float _timeToFadeSeconds;

		public event EventHandler ReleaseRequested;

		public HealthComponent Health { get; private set; }

		public void SetHealth(HealthComponent health, Transform followTransform)
		{
			// Check if the health object is the same
			if (Health == health)
				return;
			
			// Clean up old health
			if (Health)
				Health.CurrentHealthChanged -= OnHealthChanged;
			
			// Set new health
			Health = health;
			_followTransform = followTransform;

			if (!Health)
				return;

			if (Health is IHealthBarUser healthBarUser)
				_positionOffset = healthBarUser.PositionOffset;
			else
				_positionOffset = DefaultPositionOffset;
			
			UpdateImageFill(Health.CurrentHealth, Health.MaxHealth);
			UpdatePosition(); // Update the position
			_requiresUpdate = false;
			_enableCanvasFlag = true; // Enables the canvas next frame
			Health.CurrentHealthChanged += OnHealthChanged;
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

			if (_originalColors.Count > 0)
			{
				for (int imageIndex = 0; imageIndex < _imagesToFade.Count; imageIndex++)
				{
					_imagesToFade[imageIndex].color = _originalColors[imageIndex];
				}	
			}
		}

		public void CleanUp()
		{
			if (Health)
			{
				Health.CurrentHealthChanged -= OnHealthChanged;
				Health = null;
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

			for (float t = 0f; t < _timeToFadeSeconds; t += Time.deltaTime)
			{
				for (int imageIndex = 0; imageIndex < _imagesToFade.Count; imageIndex++)
				{
					Color originalColor = _originalColors[imageIndex];
					_imagesToFade[imageIndex].color = new Color(
						originalColor.r,
						originalColor.g,
						originalColor.b,
						Mathf.Lerp(originalColor.a, 0, Mathf.Min(1, t / _timeToFadeSeconds)));
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
			_imagesToFade = GetComponentsInChildren<Image>().ToList();
		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame(); // Image colors aren't set until rendering
			
			_imagesToFade.ForEach(image => _originalColors.Add(image.color));
		}

		private void Update()
		{
			if (!Health)
			{
				ReleaseRequested?.Invoke(this, EventArgs.Empty);
				return;
			}

			if (_enableCanvasFlag)
			{
				_enableCanvasFlag = false;
				_canvas.enabled = true;
			}
			
			if (_startFading)
			{
				_startFading = false;
				StartCoroutine(FadeOut());
				return;
			}

			// Update position to follow the health user
			UpdatePosition();

			// Update the image if an update is required.
			// This is used instead of updating when the value changes is due to the
			// fact that UI can't be updated on a backup thread such as an elapsed timer.
			if (_requiresUpdate)
			{
				_requiresUpdate = false;
				UpdateImageFill(Health.CurrentHealth, Health.MaxHealth);
			}
		}
	}
}