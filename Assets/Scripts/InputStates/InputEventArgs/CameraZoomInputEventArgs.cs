using InputStates.Enums;

namespace InputStates.InputEventArgs
{
	public class CameraZoomInputEventArgs : System.EventArgs
	{
		public CameraZoomInputEventArgs(ZoomType zoom)
		{
			Zoom = zoom;
		}
		
		public ZoomType Zoom { get; }
	}
}