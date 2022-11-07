using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace SubnauticaInventory.Scripts.DataModel
{
	public class Inventory
	{
		/// <summary>
		/// Grid of items where (0,0) is the top left.
		/// </summary>
		public readonly ItemData[,] ItemGrid;
		public readonly List<ItemData> Items;
		
		private int Width => ItemGrid.GetLength(0);
		private int Height => ItemGrid.GetLength(1);

		public Inventory(int width, int height)
		{
			Items = new List<ItemData>();
			ItemGrid = new ItemData[width, height];
			Clear();
		}

		public ItemData Get(int x, int y) => ItemGrid[x, y];

		/// <summary>
		/// Adds an item at a specific location (x,y)
		/// </summary>
		public void Add(ItemData itemData, int topLeftX, int topLeftY)
		{
			// Adding the same instance of an object twice is not allowed.
			Assert.IsFalse(Items.Contains(itemData));
			
			// Confirm the item can go at the given coordinates.
			for (int i = topLeftX; i < topLeftX + itemData.Width; i++)
			for (int j = topLeftY; j < topLeftY + itemData.Height; j++)
			{
				AssertValidCoordinates(i, j);
				AssertIsEmpty(i, j);
			}
			
			// Place the item at the given coordinates.
			for (int i = topLeftX; i < topLeftX + itemData.Width; i++)
			for (int j = topLeftY; j < topLeftY + itemData.Height; j++)
			{
				ItemGrid[i, j] = itemData;
			}

			Items.Add(itemData);
		}

		public void Remove(ItemData itemData)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			for (int i = 0; i < Width; i++)
			for (int j = 0; j < Height; j++)
				ItemGrid[i, j] = default;
			
			Items.Clear();
		}

		private void AssertValidCoordinates(int x, int y)
		{
			if(x >= Width)
				throw new IndexOutOfRangeException($"x value ({x}) is greater than the max x-index ({Width-1}");
			
			if(y >= Height)
				throw new IndexOutOfRangeException($"y value ({y}) is greater than the max y-index ({Height-1}");
		}

		private void AssertIsEmpty(int x, int y)
		{
			if (ItemGrid[x, y] != default)
				throw new Exception($"({x},{y}) already contains {ItemGrid[x, y]}");
		}
	}
}