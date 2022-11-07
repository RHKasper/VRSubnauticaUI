using NUnit.Framework;

namespace SubnauticaInventory.Scripts.DataModel.UnitTests
{
    public static class ItemGridTests
    {
        [Test]
        public static void TestAdd()
        {
            ItemGrid itemGrid = new ItemGrid(5,10);
            var testItem = new ItemData("Test Item", 2, 3);
            
            itemGrid.Add(testItem, 0,0);

            Assert.AreEqual(itemGrid.Get(0, 0), testItem);
            Assert.AreEqual(itemGrid.Get(1, 0), testItem);

            Assert.AreEqual(itemGrid.Get(0, 1), testItem);
            Assert.AreEqual(itemGrid.Get(1, 1), testItem);

            Assert.AreEqual(itemGrid.Get(0, 2), testItem);
            Assert.AreEqual(itemGrid.Get(1, 2), testItem);
        }
    }
}
