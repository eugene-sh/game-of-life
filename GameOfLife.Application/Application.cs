using System.Collections.Generic;
using System.Linq;
using GameOfLife.Model;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameOfLife.Application
{
	public sealed class Application
	{
		private readonly List<List<RectangleShape>> _shapes = new List<List<RectangleShape>>();
		private readonly RenderWindow _window;
		private readonly Universe _universe;
		private readonly GameParams _gameParams;

		public Application(GameParams gameParams)
		{
			_gameParams = gameParams;

			_window = new RenderWindow(
				new VideoMode(_gameParams.WindowWidth, _gameParams.WindowHeight),
				_gameParams.WindowTitle,
				Styles.Close);

			_window.Closed += (_, __) => _window.Close();
			_window.SetFramerateLimit(10);

			_universe = InitializeUniverse();
		}

		// TODO: Need to improve this thing when field editor appears.
		private Universe InitializeUniverse()
		{
			var field = new List<List<Property>>();
			for (var i = 0; i < _gameParams.CountPropertyInHeight; i++)
			{
				field.Add(new List<Property>());
				_shapes.Add(new List<RectangleShape>());

				for (var j = 0; j < _gameParams.CountPropertyInWidth; j++)
				{
					field[i].Add(new Property
					{
						State = PropertyState.Empty,
						Address = new PropertyAddress(i, j)
					});
					
					_shapes[i].Add(new RectangleShape(
						new Vector2f(_gameParams.WindowWidth, _gameParams.PropertyHeight))
					{
						FillColor = Color.White,
						OutlineColor = Color.Black,
						OutlineThickness = 1,
						Position = new Vector2f(j * _gameParams.PropertyWidth, i * _gameParams.PropertyHeight)
					});
				}
			}

			field[6][3].State = PropertyState.Populated;
			field[7][4].State = PropertyState.Populated;
			field[8][2].State = PropertyState.Populated;
			field[8][3].State = PropertyState.Populated;
			field[8][4].State = PropertyState.Populated;

			return new Universe(new UniverseParams
			{
				Size = new UniverseSize(50, 50),
				Field = field
			});
		}

		public void Start()
		{
			while (_window.IsOpen)
			{
				_window.DispatchEvents();
				_window.Clear(Color.White);

				SyncWithUniverse();

				foreach (var shape in _shapes.SelectMany(f => f))
					_window.Draw(shape);

				_window.Display();

				_universe.NextGeneration();
			}
		}

		private void SyncWithUniverse()
		{
			foreach (var (propertyAddress, property) in _universe.TrackList)
				_shapes[propertyAddress.X][propertyAddress.Y].FillColor
					= property.State == PropertyState.Empty ? Color.White : Color.Green;
		}
	}
}