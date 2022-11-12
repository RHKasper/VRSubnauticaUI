using UnityEngine;

namespace SubnauticaInventory.UI.Tooltips.SwapTooltips
{
	public class SwapTooltipController : AbstractTooltipController<ItemViewController>
	{
		public override void SetData(ItemViewController itemView)
		{
			RectTransform.parent = itemView.transform;
			RectTransform.localScale = Vector3.one;
			RectTransform.anchoredPosition = default;
			gameObject.SetActive(true);
		}
	}
}