using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace SubnauticaInventory.Scripts.DataModel.UnitTests
{
    public class BasicOperations
    {
        [Test]
        public static void TestAdd()
        {
            Inventory inventory = new Inventory(5,10);
            var testItem = new ItemData("Test Item", 2, 3);
            
            inventory.Add(testItem, 0,0);

            Assert.AreEqual(inventory.Get(0, 0), testItem);
            Assert.AreEqual(inventory.Get(1, 0), testItem);

            Assert.AreEqual(inventory.Get(0, 1), testItem);
            Assert.AreEqual(inventory.Get(1, 1), testItem);

            Assert.AreEqual(inventory.Get(0, 2), testItem);
            Assert.AreEqual(inventory.Get(1, 2), testItem);
        }
    }
}
