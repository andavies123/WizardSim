using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenuInjections : MonoBehaviour
	{
		private readonly ConcurrentDictionary<string, ContextMenuTreeNode> _rootNodesByType = new();

		public void InjectContextMenuOption<TFor>(string path, Action<TFor> menuClickCallback, Func<TFor, bool> isEnabledFunc = null, Func<TFor, bool> isVisibleFunc = null) where TFor : IContextMenuUser
		{
			string typeName = typeof(TFor).Name;
			
			if (!_rootNodesByType.TryGetValue(typeName, out ContextMenuTreeNode rootNode))
			{
				rootNode = new ContextMenuTreeNode();
				_rootNodesByType[typeName] = rootNode;
			}

			ContextMenuTreeNode childNode = ContextMenuTreeUtility.Insert(rootNode, path);
			childNode.MenuClickCallback = obj => menuClickCallback.Invoke((TFor) obj);

			if (isEnabledFunc != null)
				childNode.IsEnabledFunc = obj => isEnabledFunc.Invoke((TFor) obj);

			if (isVisibleFunc != null)
				childNode.IsVisibleFunc = obj => isVisibleFunc.Invoke((TFor) obj);
		}

		public bool TryGetRootNode(Type targetType, out ContextMenuTreeNode rootNode)
		{
			return _rootNodesByType.TryGetValue(targetType.Name, out rootNode);
		}
	}
}