using TaskSystem.Interfaces;
using GameWorld.Characters.Wizards.States;

namespace GameWorld.Characters.Wizards.Tasks
{
	public interface IWizardTask : ITask
	{
		/// <summary>
		/// A collection of allowed wizard types that are
		/// allowed to do this task
		/// </summary>
		public WizardType[] AllowedWizardTypes { get; }
		
		/// <summary>
		/// When set to true, this task will allow any wizard type to handle it
		/// and <see cref="AllowedWizardTypes"/> is deemed unnecessary
		/// </summary>
		public bool AllowAllWizardTypes { get; }
		
		/// <summary>
		/// The wizard state that is attached to this task
		/// </summary>
		public WizardTaskState WizardTaskState { get; }
		
		/// <summary>
		/// The wizard that has been assigned to this task.
		/// Null if no wizards have been assigned
		/// </summary>
		public Wizard AssignedWizard { get; set; }
	}
}