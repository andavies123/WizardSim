using Extensions;
using HealthComponents;
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
		private Health _health;
        
		public void SetHealth(Health health)
		{
			// Check if the health object is the same
			if (_health == health)
				return;
			
			// Clean up current health
			if (_health)
				_health.CurrentHealthChanged -= OnHealthChanged;
			
			// Set new health
			_health = health;
			if (_health)
				_health.CurrentHealthChanged += OnHealthChanged;
			
			// Initialize the UI
			if (_health)
				UpdateImageFill(_health.CurrentHealth, _health.MaxHealth);
			
			// Update the canvas
			_canvas.enabled = _health;
		}

		private void UpdateImageFill(float currentHealth, float maxHealth)
		{
			float healthPercentage = currentHealth.PercentageOf01(maxHealth);
			fillImage.fillAmount = healthPercentage;
			fillImage.color = colorGradient.Evaluate(healthPercentage);
		}

		private void OnHealthChanged(object sender, HealthChangedEventArgs args) => UpdateImageFill(args.CurrentHealth, _health.MaxHealth);

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
			if (_health)
				_transform.position = _health.transform.position + Vector3.up * 1.5f;
		}
	}
}