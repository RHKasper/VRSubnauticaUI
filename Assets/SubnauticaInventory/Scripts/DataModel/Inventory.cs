using System.Collections.Generic;
using UnityEngine;

namespace SubnauticaInventory.Scripts.DataModel
{
	public class Inventory
	{
		public List<ItemData> Items;
		public Dictionary<ItemData, Vector2Int> CurrentPack;

		private readonly int _width;
		private readonly int _height;

		public Inventory(int width, int height)
		{
			_width = width;
			_height = height;
			Items = new List<ItemData>();
		}

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

		private void Repack() => CurrentPack = BinPackingUtility.GenerateBinPack(Items, _width, _height);
		private bool CanAdd(ItemData itemData)
		{
			return !Items.Contains(itemData) && BinPackingUtility.ItemsFitInBin(new List<ItemData>(Items) { itemData }, _width, _height);
		}

		private void Add(ItemData itemData)
		{
			Items.Add(itemData);
			Repack();
		}
	}
}