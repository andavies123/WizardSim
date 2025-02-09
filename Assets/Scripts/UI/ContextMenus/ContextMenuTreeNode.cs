using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;

namespace UI.ContextMenus
{
	public static class ContextMenuTreeUtility
	{
		public const char PATH_SEPARATOR = '|';
		
		public static ContextMenuTreeNode Insert(ContextMenuTreeNode rootNode, string path)
		{
			if (rootNode == null || path.IsNullOrEmpty())
				throw new InvalidOperationException();

			ContextMenuTreeNode localRoot = rootNode;
			string[] textNodes = path.Split(PATH_SEPARATOR);
			
			ContextMenuTreeNode childNode = null;
			
			foreach (string textNode in textNodes)
			{
				childNode = localRoot.Children.FirstOrDefault(child => child.Text == textNode);

				if (childNode == null)
				{
					childNode = new ContextMenuTreeNode {Text = textNode};
					localRoot.AddChild(childNode);
				}
				
				localRoot = childNode;
			}
			
			return childNode;
		}
	}
	
	public class ContextMenuTreeNode
	{
		public static readonly Func<IContextMenuUser, bool> AlwaysTrue = _ => true;
		public static readonly Func<IContextMenuUser, bool> AlwaysFalse = _ => false;
		
		public string Text { get; set; }
		public Action<IContextMenuUser> MenuClickCallback { get; set; }
		public Func<IContextMenuUser, bool> IsEnabledFunc { get; set; } = AlwaysTrue;
		public Func<IContextMenuUser, bool> IsVisibleFunc { get; set; } = AlwaysTrue;
		
		public ContextMenuTreeNode ParentNode { get; set; }
		public List<ContextMenuTreeNode> Children { get; set; } = new();
		
		public bool IsLeafNode => Children.Count == 0;

		public void AddChild(ContextMenuTreeNode childNode)
		{
			childNode.ParentNode = this;
			Children.Add(childNode);
		}

		public bool CalculateVisibility(IContextMenuUser user)
		{
			if (IsLeafNode)
				return IsVisibleFunc.Invoke(user);

			return Children.Any(child => child.CalculateVisibility(user));
		}
	}
}