namespace GameWorld.Settlements
{
	public class Settlement : ISettlement
	{
		public Settlement(World parentWorld)
		{
			ParentWorld = parentWorld;
		}

		public World ParentWorld { get; }
		public IWizardRepo WizardRepo { get; } = new WizardRepo();
	}
}