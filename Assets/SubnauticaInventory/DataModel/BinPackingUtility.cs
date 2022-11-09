using System;
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
			items.Sort((a, b) => Mathf.CeilToInt(b.Height + b.Width/2f) - Mathf.CeilToInt(a.Height + a.Width/2f));
			
			// Track open spaces as rects and start with the whole bin open
			LinkedList<IntRect> openSpacesRight = new();
			openSpacesRight.AddFirst(new IntRect(0, 0, binWidth, binHeight));
			
			LinkedList<IntRect> openSpacesBelow = new();

			// Iterate through items and pack each in the first free space that can fit it, starting with top-most spaces.
			for (int i = 0; i < items.Count; i++)
			{
				var itemData = items[i];

				bool matchFound = false;
				
				// Check right spaces first so rows can be completed
				foreach (IntRect openSpace in openSpacesRight)
				{
					if (openSpace.CanFitItem(itemData))
					{
						binPack[itemData] = new Vector2Int(openSpace.X, openSpace.Y);
						ConsumeSpace(itemData, openSpace, openSpacesRight, openSpacesRight, openSpacesBelow);
						matchFound = true;
						break;
					}
				}

				// If no match in right spaces, check below spaces
				if (!matchFound)
				{
					foreach (IntRect openSpace in openSpacesBelow)
					{
						if (openSpace.CanFitItem(itemData))
						{
							binPack[itemData] = new Vector2Int(openSpace.X, openSpace.Y);
							ConsumeSpace(itemData, openSpace, openSpacesBelow, openSpacesRight, openSpacesBelow);
							break;
						}
					}
				}

				// If no spot is found for an item, this is not a valid pack.
				if (!binPack.ContainsKey(itemData))
					return null;
			}

			return binPack;
		}

		private static void ConsumeSpace(ItemData itemData, IntRect openSpace, LinkedList<IntRect> sourceList, 
			LinkedList<IntRect> openSpacesRight, LinkedList<IntRect> openSpacesBelow)
		{
			// Remove this space and replace it with resulting spaces.
			openSpace.SplitAroundItem(itemData, out IntRect? right, out IntRect? below);
			sourceList.Remove(openSpace);

			if (right.HasValue)
				openSpacesRight.AddLast(right.Value);
			if (below.HasValue)
				openSpacesBelow.AddLast(below.Value);
		}
	}
}