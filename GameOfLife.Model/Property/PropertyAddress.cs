namespace GameOfLife.Model
{
	public class PropertyAddress
	{
		public PropertyAddress(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; }
		public int Y { get; }
	}
}