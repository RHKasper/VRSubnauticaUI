using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SubnauticaInventory.DataModel;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace SubnauticaInventory.UI
{
	public class InventoryViewController : MonoBehaviour, IDropTarget
	{
		[Header("Settings")] 
		[SerializeField] private InventoryViewController transferTarget;
		[SerializeField] private float cellSize = 80;
		[SerializeField] private float spacing = 8;
		[SerializeField] private int initialSetupId = 2;

		[Header("External References")] 
		[SerializeField] private PdaViewController pdaViewController;
		
		[FormerlySerializedAs("itemsParent")]
		[Header("Internal References")]
		[SerializeField] private Transform itemViewsParent;
		[SerializeField] private GridBackgroundController gridBackgroundController;
		[SerializeField] private ItemViewController itemViewPrefab;
		
		private readonly LinkedList<ItemViewController> _activeItemViews = new();
		private readonly LinkedList<ItemViewController> _itemViewPool = new();
		private readonly Dictionary<ItemData, ItemViewController> _cachedInactiveItemViews = new();
		

		public Transform ItemViewsParent => itemViewsParent;
		public Canvas PdaOverlayCanvas => pdaViewController.OverlayCanvas;
		public PdaViewController Pda => pdaViewController;
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
		private IEnumerator Start()
		{
			yield return new WaitForSeconds(1);
			switch (initialSetupId)
			{
				case 1: TestInventoryLoad1(); break;
				case 2: TestInventoryLoad2(); break;
				case 3: TestInventoryLoad3(); break;
				case 4: TestInventoryLoad4(); break;
				default: LoadInventoryContents(new Inventory(6,8)); break;
			}
		}

		/// <summary>
		/// Returns all active <see cref="ItemViewController"/> objects from this <see cref="InventoryViewController"/>
		/// to the pool.
		/// </summary>
		private void Clear()
		{
			while (_activeItemViews.Any()) 
				ReturnItemViewToPool(_activeItemViews.Last.Value);
			
			// foreach (ItemViewController view in _cachedInactiveItemViews.Values) 
			// 	_itemViewPool.AddLast(view);
			// _cachedInactiveItemViews.Clear();
		}

		/// <summary>
		/// Creates an item view for the given <see cref="itemData"/> at the given <see cref="coordinates"/>
		/// </summary>
		private ItemViewController InstantiateItemView(ItemData itemData, Vector2Int coordinates)
		{
			ItemViewController itemView = null;

			if (_cachedInactiveItemViews.ContainsKey(itemData))
			{
				itemView = _cachedInactiveItemViews[itemData];
				itemView.gameObject.SetActive(true);
				_cachedInactiveItemViews.Remove(itemData);
			}
			else if (_itemViewPool.Any())
			{
				itemView = _itemViewPool.Last.Value;
				itemView.gameObject.SetActive(true);
				_itemViewPool.RemoveLast();
			}
			else
			{
				itemView = Instantiate(itemViewPrefab, itemViewsParent);
			}

			itemView.SetData(itemData, this);
			itemView.desiredAnchoredPosition = (cellSize * (Vector2)coordinates + Vector2.one * spacing/2).WithNegativeY();

			_activeItemViews.AddLast(itemView);
			return itemView;
		}

		private void ReturnItemViewToPool(ItemViewController itemViewController)
		{
			_activeItemViews.Remove(itemViewController);
			_cachedInactiveItemViews[itemViewController.ItemData] = itemViewController;
			//_itemViewPool.AddLast(itemViewController);
			
			itemViewController.gameObject.SetActive(false);
		}

		#region Test Methods

		[ContextMenu("Test Inventory Load 1")]
		public void TestInventoryLoad1()
		{
			Random.InitState(1);
			
			Inventory inventory = new Inventory(3, 5);
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			
			LoadInventoryContents(inventory);
		}
		
		[ContextMenu("Test Inventory Load 2")]
		public void TestInventoryLoad2()
		{
			Random.InitState(2);
			
			Inventory inventory = new Inventory(6, 8);
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			
			LoadInventoryContents(inventory);
		}
		
		[ContextMenu("Test Inventory Load 3")]
		public void TestInventoryLoad3()
		{
			Random.InitState(3);
			
			Inventory inventory = new Inventory(8, 8);
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			
			LoadInventoryContents(inventory);
		}
		
		[ContextMenu("Test Inventory Load 4")]
		public void TestInventoryLoad4()
		{
			Random.InitState(4);
			
			Inventory inventory = new Inventory(6, 8);
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem());
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			inventory.RequestAdd(SubnauticaInventoryItemLibrary.GetRandomItem(1,1));
			
			LoadInventoryContents(inventory);
		}

		#endregion

		public string Name => gameObject.name;
	}
}