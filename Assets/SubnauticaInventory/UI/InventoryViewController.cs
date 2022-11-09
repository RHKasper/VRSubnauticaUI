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
		[SerializeField] private Vector2 spacing = new(8,8);

		private readonly LinkedList<ItemViewController> _itemViewPool = new();
		private readonly LinkedList<ItemViewController> _activeItemViews = new();

		public Vector2 GetCellDimensions() => cellDimensions;
		public Vector2 GetSpacing() => spacing;
		
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
			while (_activeItemViews.Any())
			{
				ReturnItemViewToPool(_activeItemViews.Last.Value);
			}
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
				itemView.gameObject.SetActive(true);
				_itemViewPool.RemoveLast();
			}
			else
			{
				itemView = Instantiate(itemViewPrefab, itemsParent);
			}

			itemView.SetData(itemData, this);
			itemView.RectTransform.anchoredPosition = (coordinates.Multiply(cellDimensions) + spacing/2).WithNegativeY();

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

		#region Test Methods

		public Sprite[] testSprites;
		
		[ContextMenu("Test Inventory Load 1")]
		public void TestInventoryLoad1()
		{
			Inventory inventory = new Inventory(3, 5);
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 3, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 2, 3, testSprites.GetRandom()));
			
			LoadInventoryContents(inventory);
		}
		
		[ContextMenu("Test Inventory Load 2")]
		public void TestInventoryLoad2()
		{
			Inventory inventory = new Inventory(6, 8);
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 5, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 3, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 2, 3, testSprites.GetRandom()));
			
			LoadInventoryContents(inventory);
		}
		
		[ContextMenu("Test Inventory Load 3")]
		public void TestInventoryLoad3()
		{
			Inventory inventory = new Inventory(8, 8);
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 5, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 5, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 3, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 2, 3, testSprites.GetRandom()));
			
			LoadInventoryContents(inventory);
		}
		
		[ContextMenu("Test Inventory Load 4")]
		public void TestInventoryLoad4()
		{
			Inventory inventory = new Inventory(6, 8);
			inventory.RequestAdd(new ItemData("TestItem", 3, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 2, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 2, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			inventory.RequestAdd(new ItemData("TestItem", 1, 1, testSprites.GetRandom()));
			
			LoadInventoryContents(inventory);
		}

		#endregion
	}
}