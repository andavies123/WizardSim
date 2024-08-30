using System;
using Game.MessengerSystem;
using UnityEngine;

namespace UI.Messages
{
	public class StartInteractionRequest : Message
	{
		public Action<MonoBehaviour> InteractionCallback { get; set; }
	}
}