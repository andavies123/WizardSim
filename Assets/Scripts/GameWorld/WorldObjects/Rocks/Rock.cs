using System.Collections.Generic;
using System.Linq;
using Game.MessengerSystem;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using UI.ContextMenus;
using UnityEngine;
using GameWorld.Characters.Wizards.Messages;
using GameWorld.Characters.Wizards.Tasks;

namespace GameWorld.WorldObjects.Rocks
{
	[RequireComponent(typeof(WorldObject))]
	[RequireComponent(typeof(ContextMenuUser))]
	public class Rock : MonoBehaviour
	{
		public WorldObject WorldObject { get; private set; }
		private ContextMenuUser ContextMenuUser { get; set; }

		private void Awake()
		{
			WorldObject = GetComponent<WorldObject>();
			ContextMenuUser = GetComponent<ContextMenuUser>();
			InitializeContextMenu();
		}

		private void InitializeContextMenu()
		{
			ContextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Destroy", "Single"),
				() => MessageBroker.PublishSingle(new AddWizardTaskRequest
				{
					Sender = this,
					Task = new DestroyRocksTask(new List<Rock> {this})
				}),
				() => true,
				() => true);
			
			ContextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Destroy", "Surrounding"),
				() => MessageBroker.PublishSingle(new AddWizardTaskRequest
				{
					Sender = this,
					Task = new DestroyRocksTask(GetSurroundingRocks())
				}),
				() => true,
				() => true);
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