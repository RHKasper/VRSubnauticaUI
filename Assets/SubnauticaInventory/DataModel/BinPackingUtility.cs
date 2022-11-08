using System.Collections.Generic;
using UnityEngine;

namespace SubnauticaInventory.DataModel
{
	public static class BinPackingUtility
	{
		public static bool ItemsFitInBin(List<ItemData> items, int binWidth, int binHeight)
		{
			return GenerateBinPack(items, binWidth, binHeight) != null;
		}
		
		
		/// <summary>
		/// Generates a valid bin pack for the given parameters, if there is one.
		/// Source: https://codeincomplete.com/articles/bin-packing/
		/// </summary>
		/// <returns> null if there is no valid pack, otherwise returns a valid pack for the given <see cref="items"/>
		/// into a bin with a given <see cref="binWidth"/> and <see cref="binHeight"/></returns>
		public static Dictionary<ItemData, Vector2Int> GenerateBinPack(List<ItemData> items, int binWidth, int binHeight)
		{
			// Initialize Dictionary
			Dictionary<ItemData, Vector2Int> binPack = new(); 

			// Sort by height
			items.Sort((a, b) => b.Height - a.Height);
			
			// Track open spaces as rects and start with the whole bin open
			List<IntRect> openSpaces = new() { new IntRect(0, 0, binWidth, binHeight) };
			
			// Iterate through items and pack each in the first free space that can fit it
			for (int i = 0; i < items.Count; i++)
			{
				var itemData = items[i];

				foreach (IntRect openSpace in openSpaces)
				{
					if (openSpace.CanFitItem(itemData))
					{
						// Store position
						binPack[itemData] = new Vector2Int(openSpace.X, openSpace.Y);
						
						// Remove this space and replace it with resulting spaces.
						openSpace.SplitAroundItem(itemData, out IntRect? right, out IntRect? below);
						openSpaces.Remove(openSpace);
						if(right.HasValue)
							openSpaces.Add(right.Value);
						if (below.HasValue)
							openSpaces.Add(below.Value);
						
						break;
					}
				}

				// If no spot is found for an item, this is not a valid pack.
				if (!binPack.ContainsKey(itemData))
					return null;
			}

			return binPack;
		}
	}
}