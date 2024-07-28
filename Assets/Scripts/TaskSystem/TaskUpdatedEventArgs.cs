using System;

namespace TaskSystem
{
	public class TaskUpdatedEventArgs : EventArgs
	{
		public TaskUpdatedEventArgs(string propertyName)
		{
			PropertyName = propertyName;
		}
		
		public string PropertyName { get; }
	}
}