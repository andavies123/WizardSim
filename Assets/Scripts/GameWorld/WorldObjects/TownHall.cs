using GeneralBehaviours.Utilities.ContextMenuBuilders;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	public class TownHall : WorldObject
	{
		public override Vector3Int Size { get; } = new(1, 3, 1);
		public override Vector3 InitialPositionOffset { get; } = new(0.5f, 3, 0.5f);
		public override int MaxAllowed => 1;
		protected override string ItemName => "Town Hall";
		
		protected override void InitializeContextMenu()
		{
			ContextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Print Info"),
				() => print("I'M SORRY I DON'T HAVE ANY INFO FOR YOU"),
				() => true,
				() => true);
		}
	}
}