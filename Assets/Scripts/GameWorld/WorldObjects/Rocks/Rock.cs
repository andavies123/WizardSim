using System.Collections.Generic;
using System.Linq;
using Game.MessengerSystem;
using UI;
using UI.ContextMenus;
using UnityEngine;
using Wizards.Messages;
using Wizards.Tasks;

namespace GameWorld.WorldObjects.Rocks
{
	[RequireComponent(typeof(Interactable))]
	[RequireComponent(typeof(ContextMenuUser))]
	public class Rock : WorldObject
	{
		private Interactable _interactable;
		private ContextMenuUser _contextMenuUser;
		
		private void Awake()
		{
			_interactable = GetComponent<Interactable>();
			_contextMenuUser = GetComponent<ContextMenuUser>();
		}

		private void Start()
		{
			InitializeInteractable();
			InitializeContextMenu();
		}

		private void InitializeInteractable()
		{
			_interactable.TitleText = "Rock";
			_interactable.InfoText = "Just a Rock";
		}

		private void InitializeContextMenu()
		{
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Destroy", 
				() => GlobalMessenger.Publish(new AddWizardTaskRequest(new DestroyRocksTask(new List<Rock> {this})))));
			
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Destroy Surrounding",
				() => GlobalMessenger.Publish(new AddWizardTaskRequest(new DestroyRocksTask(GetSurroundingRocks())))));
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