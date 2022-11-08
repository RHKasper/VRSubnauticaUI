using SubnauticaInventory.DataModel;
using SubnauticaInventory.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace SubnauticaInventory.UI
{
	public class ItemViewController : SimpleUiBehavior
	{
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
			RectTransform.sizeDelta = inventoryViewController.GetCellDimensions().Multiply(_itemData.GetDimensions());
		}
	}
}