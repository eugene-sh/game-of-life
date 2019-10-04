namespace GameOfLife.Application
{
	internal static class Program
	{
		private static void Main()
		{
			var gameParams = new GameParams
			{
				WindowTitle = "Game Of Life",
				WindowWidth = 500,
				WindowHeight = 500,
				PropertyHeight = 10,
				PropertyWidth = 10
			};

			new Application(gameParams).Start();
		}
	}
}