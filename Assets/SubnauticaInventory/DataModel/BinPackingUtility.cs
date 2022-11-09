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
			LinkedList<IntRect> openSpaces = new();
			openSpaces.AddFirst(new IntRect(0, 0, binWidth, binHeight));

			// Iterate through items and pack each in the first free space that can fit it, starting with top-most spaces.
			foreach (var itemData in items)
			{
				// Check right spaces first so rows can be completed
				foreach (IntRect openSpace in openSpaces)
				{
					if (openSpace.CanFitItem(itemData))
					{
						binPack[itemData] = new Vector2Int(openSpace.X, openSpace.Y);
						
						// Remove this space and replace it with resulting spaces.
						openSpace.SplitAroundItem(itemData, out IntRect? right, out IntRect? below);
						openSpaces.Remove(openSpace);

						if (right.HasValue)
							AddOpenSpace(right.Value, openSpaces);
						if (below.HasValue)
							AddOpenSpace(below.Value, openSpaces);
						break;
					}
				}
				
				// If no spot is found for an item, this is not a valid pack.
				if (!binPack.ContainsKey(itemData))
					return null;
			}

			return binPack;
		}

		/// <summary>
		/// Adds an open space such that <see cref="openSpaces"/> remains sorted from least to greatest Y position
		/// </summary>
		public static void AddOpenSpace(IntRect spaceToAdd, LinkedList<IntRect> openSpaces)
		{
			if (openSpaces.Count == 0)
				openSpaces.AddFirst(spaceToAdd);
			else
			{
				LinkedListNode<IntRect> targetLocationPrecursor = openSpaces.First;

				while (spaceToAdd.Y <= targetLocationPrecursor.Value.Y)
				{
					if(targetLocationPrecursor.Next == null)
						break;
					
					targetLocationPrecursor = targetLocationPrecursor.Next;
				}

				openSpaces.AddAfter(targetLocationPrecursor, spaceToAdd);
			}
		}
	}
}