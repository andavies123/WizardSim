using System;
using UI;

namespace CameraComponents
{
	public class InteractableRaycasterEventArgs : EventArgs
	{
		public InteractableRaycasterEventArgs(Interactable interactable) => Interactable = interactable;
		
		public Interactable Interactable { get; }
	}
}