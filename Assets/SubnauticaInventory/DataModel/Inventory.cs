using System.Collections.Generic;
using UnityEngine;

namespace SubnauticaInventory.DataModel
{
	public class Inventory
	{
		public List<ItemData> Items;
		public Dictionary<ItemData, Vector2Int> CurrentPack;

		public readonly int Width;
		public readonly int Height;

		public Inventory(int width, int height)
		{
			Width = width;
			Height = height;
			Items = new List<ItemData>();
		}

		#region Public Methods

		/// <summary>
		/// Adds an item, if possible.
		/// </summary>
		/// <returns>whether the item was added</returns>
		public bool RequestAdd(ItemData itemData)
		{
			if (CanAdd(itemData))
			{
				Add(itemData);
				return true;
			}

			return false;
		}

		public void Remove(ItemData itemData)
		{
			Items.Remove(itemData);
			Repack();
		}

		/// <summary>
		/// Swaps two items, if possible.
		/// </summary>
		/// <returns>whether the item was swapped</returns>
		public bool RequestSwap(ItemData myItem, Inventory otherInventory, ItemData otherItem)
		{
			if (BinPackingUtility.ItemsCanSwap(this, myItem, otherInventory, otherItem))
			{
				Remove(myItem);
				otherInventory.Remove(otherItem);
			
				Add(otherItem);
				otherInventory.Add(myItem);
				return true;
			}

			return false;
		}

		public Vector2Int GetDimensions() => new(Width, Height); 
		
		#endregion

		#region Private Methods

		private void Repack() => CurrentPack = BinPackingUtility.GenerateBinPack(Items, Width, Height);
		private bool CanAdd(ItemData itemData)
		{
			return !Items.Contains(itemData) && BinPackingUtility.ItemsFitInBin(new List<ItemData>(Items) { itemData }, Width, Height);
		}

		private void Add(ItemData itemData)
		{
			Items.Add(itemData);
			Repack();
		}
		
		#endregion
	}
}