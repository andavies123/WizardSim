using System;
using DamageTypes;

namespace Game
{
	public class ResourceLoader
	{
		public void LoadAllResources()
		{
			LoadResource<DamageType>("Damage Type", dt => dt.Name, "Data/DamageTypes");
		}

		private void LoadResource<TRepoType>(string typeName, Func<TRepoType, string> keyGetter, params string[] resourcePaths)
		{
			ResourceRepo<TRepoType> repo = new(keyGetter, typeName);
			foreach (string resourcePath in resourcePaths)
			{
				repo.Load(resourcePath);
			}
			Dependencies.Register(repo);
		}
	}
}