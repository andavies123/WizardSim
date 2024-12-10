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

		private readonly ConcurrentDictionary<HealthComponent, HealthBarTimerPair> _healthChangedHealthBars = new();
		private HealthBar _hoverHealthBar;
		private HealthBarFactory _healthBarFactory;

		public void SetHoverHealthBar(HealthComponent health, Transform followTransform)
		{
			if (_healthChangedHealthBars.TryGetValue(health, out HealthBarTimerPair hbtPair))
			{
				_hoverHealthBar = hbtPair.HealthBar;
				return;
			}
			
			if (!_hoverHealthBar)
			{
				_hoverHealthBar = _healthBarFactory.GetHealthBar(health.transform.position);
				_hoverHealthBar.gameObject.name = "Hover Health Bar";
			}

			_hoverHealthBar.SetHealth(health, followTransform);
		}

		public void RemoveHoverHealthBar()
		{
			if (!_hoverHealthBar)
				return;

			if (!_healthChangedHealthBars.TryGetValue(_hoverHealthBar.Health, out HealthBarTimerPair _))
				_healthBarFactory.ReleaseHealthBar(_hoverHealthBar);

			_hoverHealthBar = null;
		}

		private void Awake()
		{
			_healthBarFactory = GetComponent<HealthBarFactory>();
		}

		private void Start()
		{
			HealthComponent.AnyHealthChanged += OnAnyHealthChanged;
		}

		private void OnAnyHealthChanged(object sender, EventArgs args)
		{
			if (sender is not HealthComponent health)
				return;

			if (_hoverHealthBar && _hoverHealthBar.Health == health)
				return;
			
			if (_healthChangedHealthBars.TryGetValue(health, out HealthBarTimerPair hbtPair))
			{
				hbtPair.FadeStartTimer.Stop();
			}
			else
			{
				hbtPair = new HealthBarTimerPair
				{
					HealthBar = _healthBarFactory.GetHealthBar(health.transform.position),
					FadeStartTimer = CreateTimer()
				};
				
				hbtPair.HealthBar.gameObject.name = $"Health Bar - {health.gameObject.name}";
				hbtPair.HealthBar.SetHealth(health, health.transform);
				hbtPair.HealthBar.ReleaseRequested += OnHealthBarReleaseRequested;
				hbtPair.FadeStartTimer.Elapsed += (_, _) => OnHealthBarTimerElapsed(hbtPair);

				if (!_healthChangedHealthBars.TryAdd(health, hbtPair))
					Debug.LogError("Unable to add health bar...");
			}

			hbtPair.FadeStartTimer.Start();
		}

		private void OnHealthBarTimerElapsed(HealthBarTimerPair hbtPair)
		{
			hbtPair.HealthBar.BeginFading(damagedTimeToFadeSec);
		}

		private void OnHealthBarReleaseRequested(object sender, EventArgs _)
		{
			RemoveHealthBar(sender as HealthBar);
		}

		private void RemoveHealthBar(HealthBar healthBar)
		{
			if (!_healthChangedHealthBars.TryRemove(healthBar.Health, out HealthBarTimerPair removedHbtPair))
				return;
			
			removedHbtPair.FadeStartTimer.Dispose();
			
			if (healthBar != _hoverHealthBar)
				_healthBarFactory.ReleaseHealthBar(removedHbtPair.HealthBar);
		}

		private Timer CreateTimer()
		{
			return new Timer(damagedTimeToLiveSec * 1000)
			{
				AutoReset = false,
				Enabled = false
			};
		}

		private class HealthBarTimerPair
		{
			public HealthBar HealthBar { get; set;  }
			public Timer FadeStartTimer { get; set; }
		}
	}
}