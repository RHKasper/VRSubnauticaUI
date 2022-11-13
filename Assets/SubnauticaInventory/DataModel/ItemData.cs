using System;
using System.Collections.Generic;
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

		public int InstanceID { get; private set; } = -1;

		private static Dictionary<string, int> _idCounter = new();
		//Type? (resource, equipment, etc.)

		public ItemData(string name, int width, int height)
		{
			this.name = name;
			this.width = width;
			this.height = height;
		}

		public ItemData Clone()
		{
			return new ItemData(name, width, height)
			{
				description = description,
				sprite = sprite,
				InstanceID = GetInstanceId(name)
			};
		}


		public Vector2Int GetDimensions() => new(width, height);

		private static int GetInstanceId(string itemName)
		{
			if (_idCounter.ContainsKey(itemName))
				return ++_idCounter[itemName];
			else
			{
				_idCounter.Add(itemName, 0);
				return 0;
			}
		}
	}
}