﻿using System.Collections.Generic;
using System.Linq;
using Game.MessengerSystem;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using UI.ContextMenus;
using UnityEngine;
using Wizards.Messages;
using Wizards.Tasks;

namespace GameWorld.WorldObjects.Rocks
{
	[RequireComponent(typeof(ContextMenuUser))]
	public class Rock : WorldObject
	{
		private ContextMenuUser _contextMenuUser;

		protected override string ItemName => "Rock";

		protected override void UpdateInitialLocation()
		{
			transform.position += new Vector3(0.5f, 0.5f, 0.5f);
		}
		
		protected override void Awake()
		{
			base.Awake();
			
			_contextMenuUser = GetComponent<ContextMenuUser>();
		}

		protected override void Start()
		{
			base.Start();
            
			InitializeContextMenu();
		}

		private void InitializeContextMenu()
		{
			_contextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Destroy", "Single"),
				() => GlobalMessenger.Publish(new AddWizardTaskRequest(new DestroyRocksTask(new List<Rock> {this}))),
				() => true,
				() => true);
			
			_contextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Destroy", "Surrounding"),
				() => GlobalMessenger.Publish(new AddWizardTaskRequest(new DestroyRocksTask(GetSurroundingRocks()))),
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