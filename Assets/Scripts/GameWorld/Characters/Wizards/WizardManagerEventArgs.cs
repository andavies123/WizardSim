namespace GameWorld.Characters.Wizards
{
	public class WizardManagerEventArgs
	{
		public WizardManagerEventArgs(Wizard wizard)
		{
			Wizard = wizard;
		}
		
		public Wizard Wizard { get; }
	}
}