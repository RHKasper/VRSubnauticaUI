using UnityEngine.EventSystems;

namespace SubnauticaInventory.UI.Tooltips.ItemTooltips
{
	public class ItemTooltipProvider : AbstractTooltipsProvider<ItemTooltipController, ItemViewController>
	{
		public void UpdatePosition(ItemViewController itemView, PointerEventData eventData)
		{
			if (HasActive(itemView))
			{
				ActiveTooltips[itemView].gameObject.SetActive(true);
				ActiveTooltips[itemView].UpdatePositionInternal(eventData);
				itemView.highlightBorder.gameObject.SetActive(true);
			}
		}

		protected override void OnSuccessfulShow(ItemViewController data, ItemTooltipController prefab)
		{
			prefab.gameObject.SetActive(false);
		}

		protected override void OnSuccessfulHide(ItemViewController data, ItemTooltipController prefab)
		{
			data.highlightBorder.gameObject.SetActive(false);
		}
	}
}