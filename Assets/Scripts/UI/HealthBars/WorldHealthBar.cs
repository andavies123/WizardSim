using Extensions;
using GeneralClasses.Health.HealthEventArgs;
using GeneralClasses.Health.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HealthBars
{
	[RequireComponent(typeof(Canvas))]
	public class WorldHealthBar : MonoBehaviour
	{
		[SerializeField] private Image fillImage;
		[SerializeField] private Gradient colorGradient;

		private Canvas _canvas;
		private Transform _transform;
		private IHealth _health;
		private Transform _followTransform;
		private bool _requiresUpdate = false;
        
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
			if (_health != null)
				_health.CurrentHealthChanged += OnHealthChanged;
			
			// Initialize the UI
			if (_health != null)
				_requiresUpdate = true;
			
			// Update the canvas
			_canvas.enabled = _health != null;
		}

		public void HideHealthBar()
		{
			if (_health != null)
			{
				_health.CurrentHealthChanged -= OnHealthChanged;
				_health = null;
			}

			_followTransform = null;
			_canvas.enabled = false;
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
			
			if (!fillImage)
				Debug.Log($"{nameof(WorldHealthBar)}: {nameof(fillImage)} does not exist");
		}

		private void Update()
		{
			if (_health == null)
				return;
			
			// Update position to follow the health user
			_transform.position = _followTransform.position + Vector3.up * 1.5f;
			
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