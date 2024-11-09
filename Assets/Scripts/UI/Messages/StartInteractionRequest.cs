using System;
using MessagingSystem;
using UnityEngine;

namespace UI.Messages
{
	public class StartInteractionRequest : Message
	{
		public Action<MonoBehaviour> InteractionCallback { get; set; }
	}
}