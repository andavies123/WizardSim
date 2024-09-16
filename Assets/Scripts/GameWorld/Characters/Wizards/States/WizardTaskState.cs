﻿using System;

namespace GameWorld.Characters.Wizards.States
{
	public abstract class WizardTaskState : WizardState
	{
		public event EventHandler Completed;

		protected void CompleteTask()
		{
			Completed?.Invoke(this, EventArgs.Empty);
		}
	}
}