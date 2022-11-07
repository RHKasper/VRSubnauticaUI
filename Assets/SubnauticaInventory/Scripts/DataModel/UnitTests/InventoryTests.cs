using NUnit.Framework;
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
			Assert.AreEqual(Vector2Int.zero, inventory.CurrentPack[testItem]);
		}
	}
}