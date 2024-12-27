using GameWorld.WorldObjects;
using GameWorld.WorldResources;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Settlements
{
	// Todo: Should have reference to all settlement buildings
	public class Settlement : MonoBehaviour
	{
		[Header("Resource Objects")]
		[SerializeField, Required] private TownResourceStockpile resourceStockpile;
		
		[Header("Wizard Objects")]
		[SerializeField, Required] private WizardFactory wizardFactory;
		[SerializeField, Required] private Transform wizardContainer;

		public TownHall TownHall { get; set; }
		public ISettlementWizardManager WizardManager { get; private set; }
		public TownResourceStockpile ResourceStockpile => resourceStockpile;
		public string SettlementName { get; set; } = "Un-named Settlement";
		
		private void Awake()
		{
			WizardManager = new SettlementWizardManager(wizardFactory, wizardContainer);
			WizardManager.Init();
		}

		private void OnDestroy()
		{
			WizardManager.CleanUp();
		}
	}
}