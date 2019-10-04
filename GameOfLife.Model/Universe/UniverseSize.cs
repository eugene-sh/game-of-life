namespace GameOfLife.Model
{
	public class UniverseSize
	{
		public UniverseSize(int height, int width)
		{
			Height = height;
			Width = width;
		}

		public int Height { get; }
		public int Width { get; }
	}
}