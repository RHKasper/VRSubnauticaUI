using SubnauticaInventory.DataModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SubnauticaInventory.UI
{
	public class ItemViewController : SimpleUiBehavior, IPointerDownHandler
	{
		[SerializeField] private FreeModifier[] freeModifiers;
		[SerializeField] private Image itemImage;
		
		private ItemData _itemData;
		private InventoryViewController _owner;
		
		public void SetData(ItemData itemData, InventoryViewController inventoryViewController)
		{
			_itemData = itemData;
			_owner = inventoryViewController;
			itemImage.sprite = itemData.Sprite;
			SetSize(inventoryViewController);
		}

		/// <summary>
		/// Sets this object's size based on the cell dimensions of the given <see cref="inventoryViewController"/> 
		/// </summary>
		private void SetSize(InventoryViewController inventoryViewController)
		{
			float cellSize = inventoryViewController.GetCellSize();
			float spacing = inventoryViewController.GetSpacing();
			float borderRadius = (cellSize - spacing) / 2;
			
			Vector2 viewSize = cellSize * (Vector2)_itemData.GetDimensions();
			viewSize -= spacing * Vector2.one;
			
			RectTransform.sizeDelta = viewSize;

			foreach (FreeModifier borderModifier in freeModifiers)
				borderModifier.Radius = Vector4.one * borderRadius;
		}

		// todo: implement drag and drop
		public void OnPointerDown(PointerEventData eventData)
		{
			InventoryViewController target = _owner.GetTransferTarget();

			if (target.InventoryData.RequestAdd(_itemData))
			{
				_owner.InventoryData.Remove(_itemData);
				_owner.Refresh();
				target.Refresh();
			}
		}
	}
}