using UnityEngine;

namespace SubnauticaInventory
{
	public static class ExtensionMethods
	{
		public static Vector2 Multiply(this Vector2 v, Vector2Int v2) => new(v.x * v2.x, v.y * v2.y);
		public static Vector2 Multiply(this Vector2Int v, Vector2 v2) => new(v.x * v2.x, v.y * v2.y);
		public static Vector2 Multiply(this Vector2 v, float x, float y) => new(v.x * x, v.y * y);
		public static Vector2 WithNegativeY(this Vector2 v) => new(v.x, -v.y);

		public static T GetRandom<T>(this T[] arr) => arr[Random.Range(0, arr.Length)];
	}
}