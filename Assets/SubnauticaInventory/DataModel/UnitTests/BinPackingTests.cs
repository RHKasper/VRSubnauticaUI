using System.Collections.Generic;
using NUnit.Framework;
using SubnauticaInventory.DataModel;

namespace SubnauticaInventory.Scripts.DataModel.UnitTests
{
	public static class BinPacking
	{
		[Test]
		public static void OneObjectTest()
		{
			List<ItemData> items = new()
			{
				new ItemData("test", 2, 2)
			};
			
			Assert.IsTrue(BinPackingUtility.ItemsFitInBin(items, 2,2));
			Assert.IsTrue(BinPackingUtility.ItemsFitInBin(items, 20,2));
			Assert.IsTrue(BinPackingUtility.ItemsFitInBin(items, 20,20));
			
			Assert.IsFalse(BinPackingUtility.ItemsFitInBin(items, 1,20));
			Assert.IsFalse(BinPackingUtility.ItemsFitInBin(items, 1,2));
			Assert.IsFalse(BinPackingUtility.ItemsFitInBin(items, 1,0));
			Assert.IsFalse(BinPackingUtility.ItemsFitInBin(items, 0,0));
		}
		
		[Test]
		public static void ShapeMismatchTest()
		{
			List<ItemData> items = new()
			{
				new ItemData("wide", 2, 1),
				new ItemData("tall", 1, 2)
			};

			Assert.IsTrue(BinPackingUtility.ItemsFitInBin(items, 20,2));
			Assert.IsTrue(BinPackingUtility.ItemsFitInBin(items, 20,20));
			
			Assert.IsFalse(BinPackingUtility.ItemsFitInBin(items, 2,2));
			Assert.IsFalse(BinPackingUtility.ItemsFitInBin(items, 1,20));
		}
		
		[Test]
		public static void ManyItemsTest()
		{
			List<ItemData> items = new()
			{
				new ItemData("wide", 2, 1),
				new ItemData("tall", 1, 2),
				new ItemData("very tall", 1, 3),
				new ItemData("tall", 1, 2),
				new ItemData("very tall", 1, 3),
			};

			Assert.IsTrue(BinPackingUtility.ItemsFitInBin(items, 2,6));
			Assert.IsTrue(BinPackingUtility.ItemsFitInBin(items, 3,5));
			
			Assert.IsFalse(BinPackingUtility.ItemsFitInBin(items, 2,2));
			Assert.IsFalse(BinPackingUtility.ItemsFitInBin(items, 1,20));
		}

		[Test]
		public static void AddOpenSpaceTest()
		{
			LinkedList<IntRect> list = new LinkedList<IntRect>();
			
			BinPackingUtility.AddOpenSpace(new IntRect(0,0,1,1), list);
			Assert.IsTrue(list.First.Value.Y == 0);
			
			BinPackingUtility.AddOpenSpace(new IntRect(0,2,1,1), list);
			Assert.IsTrue(list.Last.Value.Y == 2);
			
			BinPackingUtility.AddOpenSpace(new IntRect(0,1,1,1), list);
			Assert.IsTrue(list.First.Next.Value.Y == 1);
			
			BinPackingUtility.AddOpenSpace(new IntRect(0,1,1,1), list);
			Assert.IsTrue(list.First.Next.Value.Y == 1);
			Assert.IsTrue(list.First.Next.Next.Value.Y == 1);
		}
		
		[Test]
		public static void AddOpenSpaceTest2()
		{
			LinkedList<IntRect> list = new LinkedList<IntRect>();
			
			BinPackingUtility.AddOpenSpace(new IntRect(2,2,1,1), list);
			Assert.IsTrue(list.First.Value.Y == 2);

			BinPackingUtility.AddOpenSpace(new IntRect(5,0,1,1), list);
			Assert.IsTrue(list.Last.Value.Y == 2);
			Assert.IsTrue(list.First.Value.Y == 0);
			
			BinPackingUtility.AddOpenSpace(new IntRect(0,5,1,1), list);
			Assert.IsTrue(list.Last.Value.Y == 5);
			Assert.IsTrue(list.First.Value.Y == 0);
			Assert.IsTrue(list.Last.Previous.Value.Y == 2);
		}
		
		
		[Test]
		public static void SwapTest1()
		{
			List<ItemData> items1 = new()
			{
				new ItemData("tall #1 from 1", 1, 2),
				new ItemData("very tall #1 from 1", 1, 3),
				new ItemData("tall #2 from 1", 1, 2),
				new ItemData("very tall #2 from 1", 1, 3),
			};
			
			List<ItemData> items2 = new()
			{
				new ItemData("tall #1 from 2", 1, 2),
				new ItemData("very tall #1 from 2", 1, 3),
				new ItemData("tall #2 from 2", 1, 2),
				new ItemData("very tall #2 from 2", 1, 3),
			};

			Inventory inventory1 = new Inventory(2, 5);
			Inventory inventory2 = new Inventory(2, 5);

			foreach (ItemData item in items1) Assert.IsTrue(inventory1.RequestAdd(item));
			foreach (ItemData item in items2) Assert.IsTrue(inventory2.RequestAdd(item));

			Assert.IsTrue(BinPackingUtility.ItemsCanSwap(inventory1, items1[0], inventory2, items2[0]));
			Assert.IsFalse(BinPackingUtility.ItemsCanSwap(inventory1, items1[0], inventory2, items2[1]));

		}
	}
}