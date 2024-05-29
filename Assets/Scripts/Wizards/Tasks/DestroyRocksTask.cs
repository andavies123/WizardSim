using System.Collections.Generic;
using GameWorld.WorldObjects.Rocks;
using Wizards.States;

namespace Wizards.Tasks
{
	public class DestroyRocksTask : WizardTask
	{
		private readonly List<Rock> _rocks;

		public DestroyRocksTask(List<Rock> rocks)
		{
			WizardTaskState = new DestroyRocksTaskState(rocks);
		}

		public override TaskWizardType[] AllowedWizardTypes { get; } = { TaskWizardType.Earth };
		public override string DisplayName => "Destroy Rocks";
	}
}