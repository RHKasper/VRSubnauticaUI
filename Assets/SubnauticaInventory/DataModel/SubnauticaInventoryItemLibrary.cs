using System.Linq;
using UnityEngine;

namespace SubnauticaInventory.DataModel
{
	[CreateAssetMenu]
	public class SubnauticaInventoryItemLibrary : ScriptableObject
	{
		[SerializeField] private ItemData[] itemData;
		private static string ResourcesPath => nameof(SubnauticaInventoryItemLibrary);
		private static SubnauticaInventoryItemLibrary _instance;

		public static SubnauticaInventoryItemLibrary Instance
		{
			get
			{
				if(!_instance)
					Load();
				return _instance;
			}
		}

		public static ItemData GetRandomItem()
		{
			return Instance.itemData.GetRandom().Clone();
		}

		public static ItemData GetRandomItem(int maxWidth, int maxHeight)
		{
			return Instance.itemData.Where(i => i.width <= maxWidth && i.height <= maxHeight).ToArray().GetRandom().Clone();
		}
		
		private static void Load()
		{
			_instance = Resources.Load<SubnauticaInventoryItemLibrary>(ResourcesPath);
		}

	}
}