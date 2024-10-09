using GeneralBehaviours.HealthBehaviours;
using System;
using System.Collections.Concurrent;
using System.Timers;
using UnityEngine;

namespace UI.HealthBars
{
	[RequireComponent(typeof(HealthBarFactory))]
	public class HealthBarManager : MonoBehaviour
	{
		[SerializeField] private double damagedTimeToLiveSec = 5f;
		[SerializeField] private float damagedTimeToFadeSec = 3f;

		private readonly ConcurrentDictionary<HealthComponent, (HealthBar, Timer)> _healthChangedHealthBars = new();
		private HealthBar _hoverHealthBar;
		private HealthBarFactory _healthBarFactory;

		public void SetHoverHealthBar(HealthComponent health, Transform followTransform)
		{
			if (!_hoverHealthBar)
			{
				_hoverHealthBar = _healthBarFactory.CreateHealthBar(Vector3.zero);
				_hoverHealthBar.gameObject.name = "Hover Health Bar";
			}

			_hoverHealthBar.SetHealth(health, followTransform);
		}

		public void RemoveHoverHealthBar()
		{
			if (!_hoverHealthBar)
				return;

			_hoverHealthBar.BeginFading(0f);
			_hoverHealthBar = null;
		}

		private void Awake()
		{
			_healthBarFactory = GetComponent<HealthBarFactory>();
		}

		private void Start()
		{
			// Raised when any health object gets changed
			HealthComponent.AnyHealthChanged += OnAnyHealthChanged;
		}

		private void OnAnyHealthChanged(object sender, EventArgs args)
		{
			if (sender is not HealthComponent healthComponent)
				return; // Should always be a HealthComponent

			if (_healthChangedHealthBars.TryGetValue(healthComponent, out (HealthBar healthBar, Timer timer) value))
			{
				value.timer.Stop();
			}
			else
			{
				value.healthBar = _healthBarFactory.CreateHealthBar(healthComponent.transform.position);
				value.healthBar.gameObject.name = $"Health Bar - {healthComponent.gameObject.name}";
				value.healthBar.SetHealth(healthComponent, healthComponent.transform);

				value.timer = CreateTimer();
				value.timer.Elapsed += (sender, _) => OnHealthBarTimerElapsed(healthComponent);

				if (!_healthChangedHealthBars.TryAdd(healthComponent, (value.healthBar, value.timer)))
					Debug.LogError("Unable to add health bar...");
			}

			value.timer.Start();
		}

		private void OnHealthBarTimerElapsed(HealthComponent healthComponent)
		{
			_healthChangedHealthBars.TryRemove(healthComponent, out (HealthBar, Timer) removed);
			(HealthBar healthBar, Timer timer) = removed;
			
			healthBar.BeginFading(damagedTimeToFadeSec);
			timer.Dispose();
		}

		private Timer CreateTimer()
		{
			Timer timer = new(damagedTimeToLiveSec * 1000)
			{
				AutoReset = false,
				Enabled = false
			};

			return timer;
		}
	}
}