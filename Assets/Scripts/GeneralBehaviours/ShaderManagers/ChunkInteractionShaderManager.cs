using System.Collections;
using System.ComponentModel;
using UI;

namespace GeneralBehaviours.ShaderManagers
{
	public class ChunkInteractionShaderManager : InteractionShaderManager
	{
		protected override void OnInteractablePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			base.OnInteractablePropertyChanged(sender, args);

			if (args.PropertyName == nameof(Interactable.IsHovered))
			{
				if (interactable.IsHovered)
					StartCoroutine(nameof(UpdateHover));
				else
					StopCoroutine(nameof(UpdateHover));
			}
		}

		private IEnumerator UpdateHover()
		{
			while (true)
			{
				yield return null;
			}
		}
	}
}