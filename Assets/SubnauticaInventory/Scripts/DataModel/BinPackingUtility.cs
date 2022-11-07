using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SubnauticaInventory.Scripts.DataModel
{
	public static class BinPackingUtility
	{
		public static bool ItemsFitInBin(List<ItemData> items, int binWidth, int binHeight)
		{
			if (items.Count == 1)
				return items[0].Width <= binWidth && items[0].Height <= binHeight;

			throw new NotImplementedException();
		}
		
		
		/// <summary>
		/// Returns a valid pack for the given <see cref="items"/> into a bin with a given <see cref="binWidth"/> and <see cref="binHeight"/>
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		/// <exception cref="InvalidDataException"></exception>
		public static Dictionary<ItemData, Vector2Int> GenerateBinPack(List<ItemData> items, int binWidth, int binHeight)
		{
			if (items.Count == 1)
			{
				if (items[0].Width <= binWidth && items[0].Height <= binHeight)
					return new Dictionary<ItemData, Vector2Int>()
					{
						{ items[0], Vector2Int.zero }
					};
				
				throw new InvalidDataException();
			}
			
			throw new NotImplementedException();
		}
	}
}