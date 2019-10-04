namespace GameOfLife.Application
{
	public class GameParams
	{
		public uint WindowWidth { get; set; }
		public uint WindowHeight { get; set; }
		public string WindowTitle { get; set; }
		public uint PropertyWidth { get; set; }
		public uint PropertyHeight { get; set; }

		public uint CountPropertyInWidth => WindowWidth / PropertyWidth;
		public uint CountPropertyInHeight => WindowHeight / PropertyHeight;
	}
}