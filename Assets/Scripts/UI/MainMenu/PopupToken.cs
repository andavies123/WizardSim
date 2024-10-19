using System;

namespace UI.MainMenu
{
	public class PopupToken
	{
		public enum CloseType { Canceled, Accepted, Rejected }
		
		/// <summary>
		/// Raised when the popup is closed
		/// </summary>
		public event Action<CloseType> PopupClosed;

		public void Accept() => PopupClosed?.Invoke(CloseType.Accepted);
		public void Reject() => PopupClosed?.Invoke(CloseType.Rejected);
		public void Cancel() => PopupClosed?.Invoke(CloseType.Canceled);
	}
}