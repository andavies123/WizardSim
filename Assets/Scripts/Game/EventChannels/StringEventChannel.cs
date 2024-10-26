using System;
using UnityEngine;

namespace Game.EventChannels
{
	[CreateAssetMenu(menuName = "Event Channel/String", fileName = "StringEventChannel", order = 0)]
	public class StringEventChannel : ScriptableObject
	{
		public event EventHandler<string> Raised;

		public void Raise(object sender, string value)
		{
			Raised?.Invoke(sender, value);
		}
	}
}