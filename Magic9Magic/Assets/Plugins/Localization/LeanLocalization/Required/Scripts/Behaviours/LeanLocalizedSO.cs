using UnityEngine;

namespace Lean.Localization
{
	/// <summary>This component will update an object, or use a fallback if none is found</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu(LeanLocalization.ComponentPathPrefix + "Localized SO")]
	public class LeanLocalizedSO : LeanLocalizedBehaviour
	{
		public ScriptableObject LocalizedSO;

		[Tooltip("If PhraseName couldn't be found, this SO will be used")]
		public ScriptableObject FallbackSO;

		// This gets called every time the translation needs updating
		public override void UpdateTranslation(LeanTranslation translation)
		{
			// Use translation?
			if (translation != null)
			{
				LocalizedSO = (ScriptableObject) translation.Data;
			}
			// Use fallback?
			else
			{
				LocalizedSO = FallbackSO;
			}
		}

		protected virtual void Awake()
		{

		}
	}
}