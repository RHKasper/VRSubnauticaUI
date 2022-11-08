namespace SubnauticaInventory.Scripts.DataModel
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
			
		public bool CanFitItem(ItemData itemData) => Width >= itemData.Width && Height >= itemData.Height;

		public void SplitAroundItem(ItemData itemData, out IntRect? right, out IntRect? below)
		{
			right = null;
			below = null;
			
			if (Width > itemData.Width)
				right = new IntRect(X + itemData.Width, Y, Width - itemData.Width, itemData.Height);
			if (Height > itemData.Height)
				below = new IntRect(X, Y + itemData.Height, itemData.Width, Height - itemData.Height);
		}
	}
}