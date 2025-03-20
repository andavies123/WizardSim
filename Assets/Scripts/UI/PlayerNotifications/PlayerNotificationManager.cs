using System.Collections.Concurrent;
using System.Timers;
using TMPro;
using UnityEngine;
using Utilities.Attributes;

namespace UI.PlayerNotifications
{
	public class PlayerNotificationManager : MonoBehaviour
	{
		[SerializeField] private Transform notificationBox;
		[SerializeField] private TMP_Text textLabel;
		
		private readonly ConcurrentQueue<PlayerNotification> _notificationQueue = new();
		private readonly Timer _notificationTimer = new() { AutoReset = false };
		
		public void AddNotification(string text, float timeToLive)
		{
			_notificationQueue.Enqueue(new PlayerNotification(text, timeToLive));
			if (_notificationQueue.Count == 1)
			{
				transform.gameObject.SetActive(true);
				DisplayNextNotification();
			}
		}

		private void DisplayNextNotification()
		{
			if (!_notificationQueue.TryDequeue(out PlayerNotification notification))
			{
				transform.gameObject.SetActive(false);
				// Todo: Fade out the notification box
				// No notifications left to display
				return;
			}
			
			textLabel.SetText(notification.Text);
			_notificationTimer.Interval = notification.TimeToLive * 1000; // Seconds to Milliseconds
			_notificationTimer.Start();
		}

		private struct PlayerNotification
		{
			public readonly string Text;
			public readonly float TimeToLive;

			public PlayerNotification(string text, float timeToLive)
			{
				Text = text;
				TimeToLive = timeToLive;
			}
		}
	}
}