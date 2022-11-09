using SubnauticaInventory.DataModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace SubnauticaInventory.UI
{
	public class ItemViewController : SimpleUiBehavior
	{
		[SerializeField] private FreeModifier borderModifier;
		[SerializeField] private Image itemImage;
		
		private ItemData _itemData;
		
		public void SetData(ItemData itemData, InventoryViewController inventoryViewController)
		{
			_itemData = itemData;
			itemImage.sprite = itemData.Sprite;
			SetSize(inventoryViewController);
		}

		/// <summary>
		/// Sets this object's size based on the cell dimensions of the given <see cref="inventoryViewController"/> 
		/// </summary>
		private void SetSize(InventoryViewController inventoryViewController)
		{
			Vector2 viewSize = inventoryViewController.GetCellDimensions().Multiply(_itemData.GetDimensions());
			Vector2 spacing = inventoryViewController.GetSpacing();
			viewSize -= spacing;

			RectTransform.sizeDelta = viewSize;
		}
	}
}