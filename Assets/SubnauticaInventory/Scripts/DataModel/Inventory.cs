using System;
using System.Collections.Generic;

namespace SubnauticaInventory.Scripts.DataModel
{
	public class Inventory
	{
		public List<ItemData> Items;

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
			throw new NotImplementedException();
		}

		private void Repack()
		{
			throw new NotImplementedException();
		}

		private bool CanAdd(ItemData itemData)
		{
			throw new NotImplementedException();
		}

		private void Add(ItemData itemData)
		{
			throw new NotImplementedException();
		}
	}
}