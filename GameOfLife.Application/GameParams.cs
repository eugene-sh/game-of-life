namespace GameOfLife.Application
{
	public class GameParams
	{
		public uint WindowWidth { get; set; }
		public uint WindowHeight { get; set; }
		public string WindowTitle { get; set; }
		public uint CellWidth { get; set; }
		public uint CellHeight { get; set; }

		public uint CountCellInWidth => WindowWidth / CellWidth;
		public uint CountCellInHeight => WindowHeight / CellHeight;
	}
}