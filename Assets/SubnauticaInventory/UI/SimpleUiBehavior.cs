using UnityEngine;
using UnityEngine.UI;

namespace SubnauticaInventory.UI
{
	/// <summary>
	/// This class allows components that inherit from it to directly access <see cref="RectTransform"/> like Unity's
	/// <see cref="Image"/>, <see cref="Text"/>, and other UI components.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public abstract class SimpleUiBehavior : MonoBehaviour
	{
		private RectTransform _rectTransform;
		public RectTransform RectTransform
		{
			get
			{
				if(!_rectTransform)
					_rectTransform = GetComponent<RectTransform>();
				return _rectTransform;
			}
		}
	}
}