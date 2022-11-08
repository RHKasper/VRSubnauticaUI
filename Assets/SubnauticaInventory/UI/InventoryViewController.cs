using System;
using System.Collections.Generic;
using System.Linq;
using SubnauticaInventory.DataModel;
using UnityEngine;

namespace SubnauticaInventory.UI
{
	public class InventoryViewController : MonoBehaviour
	{
		[SerializeField] private Transform itemsParent;
		[SerializeField] private GridBackgroundController gridBackgroundController;
		[SerializeField] private ItemViewController itemViewPrefab;
		[SerializeField] private Vector2 cellDimensions = new(60, 60);

		private readonly LinkedList<ItemViewController> _itemViewPool = new();
		private readonly LinkedList<ItemViewController> _activeItemViews = new();

		public Vector2 GetCellDimensions() => cellDimensions;
		
		/// <summary>
		/// Clears the current view and generates item views for the given inventory. 
		/// </summary>
		public void LoadInventoryContents(Inventory inventory)
		{
			Clear();
			
			gridBackgroundController.cellDimensions = cellDimensions;
			gridBackgroundController.gridLayout = inventory.GetDimensions();
			
			foreach (ItemData itemData in inventory.Items)
				InstantiateItemView(itemData, inventory.CurrentPack[itemData]);
		}

		/// <summary>
		/// Returns all active <see cref="ItemViewController"/> objects from this <see cref="InventoryViewController"/>
		/// to the pool.
		/// </summary>
		private void Clear()
		{
			foreach (ItemViewController activeItemView in _activeItemViews) 
				ReturnItemViewToPool(activeItemView);
		}

		/// <summary>
		/// Creates an item view for the given <see cref="itemData"/> at the given <see cref="coordinates"/>
		/// </summary>
		private ItemViewController InstantiateItemView(ItemData itemData, Vector2Int coordinates)
		{
			ItemViewController itemView = null;
			
			if (_itemViewPool.Any())
			{
				itemView = _itemViewPool.Last.Value;				
				_itemViewPool.RemoveLast();
			}
			else
			{
				itemView = Instantiate(itemViewPrefab, itemsParent);
			}

			itemView.SetData(itemData, this);
			itemView.RectTransform.anchoredPosition = coordinates.Multiply(cellDimensions).WithNegativeY();

			_activeItemViews.AddLast(itemView);
			return itemView;
		}

		private void ReturnItemViewToPool(ItemViewController itemViewController)
		{
			_activeItemViews.Remove(itemViewController);
			_itemViewPool.AddLast(itemViewController);
			
			itemViewController.RectTransform.anchoredPosition = Vector2.zero;
			itemViewController.gameObject.SetActive(false);
		}

		[ContextMenu("Test Inventory Load")]
		private void TestInventoryLoad()
		{
			Inventory inventory = new Inventory(5, 8);
			inventory.RequestAdd(new ItemData("TestItem", 1, 1));
			inventory.RequestAdd(new ItemData("TestItem", 5, 2));
			inventory.RequestAdd(new ItemData("TestItem", 1, 2));
			inventory.RequestAdd(new ItemData("TestItem", 3, 2));
			inventory.RequestAdd(new ItemData("TestItem", 2, 3));
			
			LoadInventoryContents(inventory);
		}
	}
}