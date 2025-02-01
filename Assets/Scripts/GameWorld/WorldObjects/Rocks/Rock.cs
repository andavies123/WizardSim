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
				_ => _messageBroker.PublishSingle(new AddWizardTaskRequest
				{
					Sender = this,
					Task = new DestroyRocksTask(new List<Rock> {this})
				}));
			
			Globals.ContextMenuInjections.InjectContextMenuOption<Rock>(
				ContextMenuBuilder.BuildPath("Destroy", "Surrounding"),
				_ => _messageBroker.PublishSingle(new AddWizardTaskRequest
				{
					Sender = this,
					Task = new DestroyRocksTask(GetSurroundingRocks())
				}));
		}

		private List<Rock> GetSurroundingRocks()
		{
			Collider[] hits = Physics.OverlapSphere(transform.position, 10);

			List<Rock> rocks = hits.Select(hit => hit.GetComponentInParent<Rock>())
								   .Where(rock => rock)
								   .ToList();

			return rocks;
		}
	}
}