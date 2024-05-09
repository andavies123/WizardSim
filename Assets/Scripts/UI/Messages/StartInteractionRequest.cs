using System;
using Game.MessengerSystem;
using UnityEngine;

namespace UI.Messages
{
	public class StartInteractionRequest : IMessage
	{
		public StartInteractionRequest(Action<MonoBehaviour> interactionCallback)
		{
			InteractionCallback = interactionCallback;
		}
		
		public Action<MonoBehaviour> InteractionCallback { get; set; }
	}
}