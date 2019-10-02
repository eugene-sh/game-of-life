using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameOfLife.Model;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameOfLife.Application
{
	internal static class Program
	{
		private static RectangleShape[,] _shapes;
		private static List<RectangleShape> _diffshapes = new List<RectangleShape>();
		
		static void Main(string[] args)
		{
			var uni = new Universe(new UniverseParams
			{
				Size = new UniverseSize
				{
					Height = 50,
					Width = 50
				}
			});

			var window = new RenderWindow(new VideoMode(500, 500), "SFML running in .NET Core");
			window.Closed += (_, __) => window.Close();
			window.SetFramerateLimit(20);

			_shapes = Shapes();
			var stop = new Stopwatch();
			stop.Start();
			
			while (window.IsOpen)
			{
				Console.WriteLine($"Start {stop.ElapsedMilliseconds}");
				window.DispatchEvents();
				window.Clear(Color.White);

				DrawField(uni, window);
				
				foreach (var f in _shapes)
				{
					window.Draw(f);
				}
				
				window.Display();
				uni.NextGeneration();
				Console.WriteLine($"End {stop.ElapsedMilliseconds}");
				_diffshapes.Clear();
			}
		}

		private static RectangleShape[,] Shapes()
		{
			var field = new RectangleShape[50, 50];

			for (var i = 0; i < 50; i++)
			{
				for (var j = 0; j < 50; j++)
				{
					field[i, j] = new RectangleShape(new Vector2f(10, 10))
					{
						FillColor = Color.White,
						OutlineColor = Color.Black,
						OutlineThickness = 1,
						Position = new Vector2f(j * 10, i * 10)
					};
				}
			}

			return field;
		}

		private static void DrawField(Universe u, RenderWindow w)
		{
			for (var i = 0; i < 50; i++)
			{
				for (var j = 0; j < 50; j++)
				{
					_shapes[i, j].FillColor = u.Field[i][j].State == PropertyState.Empty ? Color.White : Color.Green;
					
				}
			}
		}
	}
}