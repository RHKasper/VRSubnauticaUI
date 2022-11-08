using NUnit.Framework;
using SubnauticaInventory.DataModel;
using UnityEngine;

namespace SubnauticaInventory.Scripts.DataModel.UnitTests
{
	public static class InventoryTests
	{
		[Test]
		public static void TestAddOneObject()
		{
			Inventory inventory = new Inventory(10, 10);
			ItemData testItem = new ItemData("Test Item", 10, 10);

			Assert.IsTrue(inventory.RequestAdd(testItem));
			Assert.Contains(testItem, inventory.Items);
		}
		
		[Test]
		public static void TestAddSameObjectTwice()
		{
			Inventory inventory = new Inventory(10, 10);
			ItemData testItem = new ItemData("Test Item", 10, 10);

			Assert.IsTrue(inventory.RequestAdd(testItem));
			Assert.IsFalse(inventory.RequestAdd(testItem));
			Assert.Contains(testItem, inventory.Items);
		}
		
		[Test]
		public static void TestAddAndRemoveOneObject()
		{
			Inventory inventory = new Inventory(10, 10);
			ItemData testItem = new ItemData("Test Item", 10, 10);

			Assert.IsTrue(inventory.RequestAdd(testItem));
			Assert.Contains(testItem, inventory.Items);
			Assert.AreEqual(Vector2Int.zero, inventory.CurrentPack[testItem]);
			
			inventory.Remove(testItem);
			Assert.IsFalse(inventory.Items.Contains(testItem));
		}
	}
}