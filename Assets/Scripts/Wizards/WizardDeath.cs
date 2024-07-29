using System;
using Extensions;
using GeneralClasses.Health.HealthEventArgs;
using UnityEngine;

namespace Wizards
{
	[RequireComponent(typeof(Wizard))]
	public class WizardDeath : MonoBehaviour
	{
		private Wizard _wizard;

		public event EventHandler<WizardDiedEventArgs> Died;

		private void Awake()
		{
			_wizard = GetComponent<Wizard>();
		}

		private void Start()
		{
			_wizard.Health.Health.ReachedMinHealth += OnReachedMinHealth;
		}

		private void OnDestroy()
		{
			_wizard.Health.Health.ReachedMinHealth -= OnReachedMinHealth;
		}

		private void OnReachedMinHealth(object sender, ReachedMinHealthEventArgs args)
		{
			Died?.Invoke(this, new WizardDiedEventArgs(_wizard));
			_wizard.gameObject.Destroy();
		}
	}
}