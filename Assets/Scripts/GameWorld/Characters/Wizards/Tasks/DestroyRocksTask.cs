using System.Collections.Generic;
using GameWorld.WorldObjects.Rocks;
using GameWorld.Characters.Wizards.States;

namespace GameWorld.Characters.Wizards.Tasks
{
	public class DestroyRocksTask : WizardTask
	{
		public DestroyRocksTask(List<Rock> rocks)
		{
			WizardTaskState = new DestroyRocksTaskState(rocks);
		}

		public override WizardType[] AllowedWizardTypes { get; } = { WizardType.Earth };
		public override bool AllowAllWizardTypes => false;
		public override string DisplayName => "Destroying Rocks";
	}
}