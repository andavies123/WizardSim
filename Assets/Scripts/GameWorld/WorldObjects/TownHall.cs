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
	public class TownHall : MonoBehaviour, IContextMenuUser
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
			Globals.ContextMenuInjections.InjectContextMenuOption<TownHall>(
				ContextMenuBuilder.BuildPath("Open Menu"),
				_ => OpenTownHallMenu(),
				() => true,
				() => true);

			Globals.ContextMenuInjections.InjectContextMenuOption<TownHall>(
				ContextMenuBuilder.BuildPath("Print Info"),
				_ => print("I'M SORRY I DON'T HAVE ANY INFO FOR YOU"),
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