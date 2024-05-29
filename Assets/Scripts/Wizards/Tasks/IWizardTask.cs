using TaskSystem.Interfaces;
using Wizards.States;

namespace Wizards.Tasks
{
	public interface IWizardTask : ITask
	{
		/// <summary>
		/// A collection of allowed wizard types that are
		/// allowed to do this task
		/// </summary>
		public TaskWizardType[] AllowedWizardTypes { get; }
		
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