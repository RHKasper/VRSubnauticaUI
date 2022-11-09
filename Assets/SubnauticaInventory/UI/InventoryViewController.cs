using System;
using System.Collections.Generic;
using System.Linq;
using SubnauticaInventory.DataModel;
using UnityEngine;

namespace SubnauticaInventory.UI
{
	public class InventoryViewController : MonoBehaviour
	{
		[Header("Settings")] 
		[SerializeField] private InventoryViewController transferTarget;
		[SerializeField] private float cellSize = 80;
		[SerializeField] private float spacing = 8;

		[Header("Internal References")]
		[SerializeField] private Transform itemsParent;
		[SerializeField] private GridBackgroundController gridBackgroundController;
		[SerializeField] private ItemViewController itemViewPrefab;
		
		private readonly LinkedList<ItemViewController> _itemViewPool = new();
		private readonly LinkedList<ItemViewController> _activeItemViews = new();
		
		public Inventory InventoryData { get; private set; }
		public InventoryViewController TransferTarget => transferTarget;
		public float CellSize => cellSize;
		public float Spacing => spacing;
		
		/// <summary>
		/// Clears the current view and generates item views for the given inventory. 
		/// </summary>
		public void LoadInventoryContents(Inventory inventory)
		{
			Clear();
			
			gridBackgroundController.cellDimensions = new Vector2(cellSize,cellSize);
			gridBackgroundController.gridLayout = inventory.GetDimensions();
			
			foreach (ItemData itemData in inventory.Items)
				InstantiateItemView(itemData, inventory.CurrentPack[itemData]);
			
			InventoryData = inventory;
		}

		/// <summary>
		/// Refreshes UI elements. This should be called when the <see cref="InventoryData"/> object is modified.
		/// </summary>
		public void Refresh() => LoadInventoryContents(InventoryData);

		/// <summary>
		/// Initialize with an empty inventory
		/// </summary>
		private void Awake() => LoadInventoryContents(new Inventory(6,8));

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
			itemView.RectTransform.anchoredPosition = (cellSize * (Vector2)coordinates + Vector2.one * spacing/2).WithNegativeY();

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