using System.Collections.Generic;
using System.Linq;
using Game;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using UI.ContextMenus;
using UnityEngine;
using GameWorld.Characters.Wizards.Messages;
using GameWorld.Characters.Wizards.Tasks;
using MessagingSystem;

namespace GameWorld.WorldObjects.Rocks
{
	[RequireComponent(typeof(WorldObject))]
	[RequireComponent(typeof(ContextMenuUser))]
	public class Rock : MonoBehaviour, IContextMenuUser
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
			Globals.ContextMenuInjections.InjectContextMenuOption<Rock>(
				ContextMenuBuilder.BuildPath("Destroy", "Single"),
				rock => _messageBroker.PublishSingle(new AddWizardTaskRequest
				{
					Sender = rock,
					Task = new DestroyRocksTask(new List<Rock> {rock})
				}));
			
			Globals.ContextMenuInjections.InjectContextMenuOption<Rock>(
				ContextMenuBuilder.BuildPath("Destroy", "Surrounding"),
				rock => _messageBroker.PublishSingle(new AddWizardTaskRequest
				{
					Sender = rock,
					Task = new DestroyRocksTask(GetSurroundingRocks(rock.transform.position))
				}));
		}

		public static List<Rock> GetSurroundingRocks(Vector3 initialRockPosition)
		{
			Collider[] hits = Physics.OverlapSphere(initialRockPosition, 10);

			return hits.Select(hit => hit.GetComponentInParent<Rock>())
				.Where(rock => rock)
				.ToList();
		}
	}
}