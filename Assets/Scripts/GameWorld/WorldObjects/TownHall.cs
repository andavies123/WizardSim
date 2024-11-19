using Game;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using Messages.UI;
using Messages.UI.Enums;
using MessagingSystem;
using UI.ContextMenus;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	[RequireComponent(typeof(WorldObject))]
	[RequireComponent(typeof(ContextMenuUser))]
	public class TownHall : MonoBehaviour
	{
		private MessageBroker _messageBroker;

		public WorldObject WorldObject { get; private set; }
		private ContextMenuUser ContextMenuUser { get; set; }

		private void Awake()
		{
			WorldObject = GetComponent<WorldObject>();
			ContextMenuUser = GetComponent<ContextMenuUser>();
			_messageBroker = Dependencies.Get<MessageBroker>();
			InitializeContextMenu();
		}

		private void InitializeContextMenu()
		{
			ContextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Open Menu"),
				OpenTownHallMenu,
				() => true,
				() => true);

			ContextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Print Info"),
				() => print("I'M SORRY I DON'T HAVE ANY INFO FOR YOU"),
				() => true,
				() => true);
		}

		private void OpenTownHallMenu()
		{
			_messageBroker.PublishSingle(new OpenUIRequest
			{
				Sender = this,
				Window = UIWindow.TownHallWindow
			});
		}
	}
}