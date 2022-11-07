using UnityEngine;

namespace SubnauticaInventory.Scripts.DataModel
{
	public class ItemData
	{
		public string Name;
		public string Description;
		public Sprite Sprite;
		public int Width;
		public int Height;
		//Type? resource, equipment, etc.

		public ItemData(string name, int width, int height)
		{
			Name = name;
			Width = width;
			Height = height;
		}
	}
}