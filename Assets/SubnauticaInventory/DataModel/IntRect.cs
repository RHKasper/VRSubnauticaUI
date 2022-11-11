namespace SubnauticaInventory.DataModel
{
	public struct IntRect
	{
		public int X, Y, Width, Height;

		public IntRect(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}
			
		public bool CanFitItem(ItemData itemData) => Width >= itemData.width && Height >= itemData.height;

		public void SplitAroundItem(ItemData itemData, out IntRect? right, out IntRect? below)
		{
			right = null;
			below = null;
			
			if (Width > itemData.width)
				right = new IntRect(X + itemData.width, Y, Width - itemData.width, itemData.height);
			if (Height > itemData.height)
				below = new IntRect(X, Y + itemData.height, Width, Height - itemData.height);
		}
	}
}