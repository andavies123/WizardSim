using System;
using Game.MessengerSystem;
using UnityEngine;

namespace UI.Messages
{
	public class StartInteractionRequest : Message
	{
		public StartInteractionRequest(object sender, Action<MonoBehaviour> interactionCallback) : base(sender)
		{
			InteractionCallback = interactionCallback;
		}
		
		public Action<MonoBehaviour> InteractionCallback { get; }
	}
}