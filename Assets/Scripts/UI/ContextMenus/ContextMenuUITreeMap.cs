using System.Collections.Generic;

namespace UI.ContextMenus
{
	public class ContextMenuUITreeMap
	{
		private readonly Dictionary<ContextMenuTreeNode, IContextMenuUser> _usersByNode = new();
            
		public ContextMenuUITreeMap(List<(IContextMenuUser, ContextMenuTreeNode)> userTreePairs)
		{
			foreach ((IContextMenuUser contextMenuUser, ContextMenuTreeNode contextMenuTree) in userTreePairs)
			{
				BuildMap(contextMenuUser, contextMenuTree);
			}
		}

		public bool TryGetUserFromNode(ContextMenuTreeNode treeNode, out IContextMenuUser user)
		{
			user = null;
			
			if (treeNode == null)
				return false;

			return _usersByNode.TryGetValue(treeNode, out user);
		}

		private void BuildMap(IContextMenuUser menuUser, ContextMenuTreeNode treeNode)
		{
			_usersByNode.TryAdd(treeNode, menuUser);
			
			if (treeNode.IsLeafNode)
				return;

			foreach (ContextMenuTreeNode childNode in treeNode.Children)
			{
				BuildMap(menuUser, childNode);
			}
		}
	}
}