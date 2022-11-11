using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SubnauticaInventory.DataModel
{
	[Serializable]
	public class ItemData
	{
		public string name;
		public string description;
		public Sprite sprite;
		public int width;
		public int height;
		//Type? (resource, equipment, etc.)

		public ItemData(string name, int width, int height)
		{
			this.name = name;
			this.width = width;
			this.height = height;
		}
		
		public ItemData(string name, int width, int height, Sprite sprite)
		{
			this.name = name;
			this.width = width;
			this.height = height;
			this.sprite = sprite;
		}

		public ItemData Clone()
		{
			return new ItemData(name, width, height)
			{
				description = description,
				sprite = sprite
			};
		}

		public Vector2Int GetDimensions() => new(width, height);
	}
}