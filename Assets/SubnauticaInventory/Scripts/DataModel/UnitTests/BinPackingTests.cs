using System.Collections.Generic;
using NUnit.Framework;

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
	}
}