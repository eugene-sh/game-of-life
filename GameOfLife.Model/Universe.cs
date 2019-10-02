using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Model
{
	public class Universe
	{
		public List<List<Property>> Field { get; } = new List<List<Property>>();

		private readonly UniverseParams _universeParams;

		public Universe(UniverseParams universeParams)
		{
			_universeParams = universeParams;

			FieldInitialization();
		}

		private void FieldInitialization()
		{
			for (var i = 0; i < _universeParams.Size.Height; i++)
			{
				Field.Add(new List<Property>());

				for (var j = 0; j < _universeParams.Size.Width; j++)
				{
					Field[i].Add(new Property
					{
						State = PropertyState.Empty,
						Address = new PropertyAddress
						{
							X = i,
							Y = j
						}
					});
				}
			}

			Field[6][3].State = PropertyState.Populated;
			Field[7][4].State = PropertyState.Populated;
			Field[8][2].State = PropertyState.Populated;
			Field[8][3].State = PropertyState.Populated;
			Field[8][4].State = PropertyState.Populated;
		}


		private Dictionary<Property, ActionOnProperty>
			actionOnProperties = new Dictionary<Property, ActionOnProperty>();

		public Dictionary<PropertyAddress, Property> Diff = new Dictionary<PropertyAddress, Property>();

		public void NextGeneration()
		{
			var nForNext = new Dictionary<PropertyAddress, Property>();
			var nForRem = new Dictionary<PropertyAddress, Property>();
			foreach (var p in Diff)
			{
				var actionOnProperty = GetActionOnProperty(p.Key.X, p.Key.Y);

				if (actionOnProperty == ActionOnProperty.Nothing)
				{
					nForRem.Add(p.Key, p.Value);
					continue;
				}


				actionOnProperties[Field[p.Key.X][p.Key.Y]] = actionOnProperty;

				foreach (var neighbor in GetNeighbors(p.Key.X, p.Key.Y))
				{
					nForNext[neighbor.Address] = neighbor;
				}
			}

			foreach (var item in nForRem)
			{
				Diff.Remove(item.Key);
			}

			nForRem.Clear();
			foreach (var item in nForNext)
			{
				if (!Diff.ContainsKey(item.Key))
				{
					Diff.Add(item.Key, item.Value);
				}
			}

			nForNext.Clear();

			if (!Diff.Any())
				for (var i = 0; i < _universeParams.Size.Height; i++)
				{
					for (var j = 0; j < _universeParams.Size.Width; j++)
					{
						var actionOnProperty = GetActionOnProperty(i, j);

						if (actionOnProperty == ActionOnProperty.Nothing)
							continue;

						actionOnProperties[Field[i][j]] = actionOnProperty;

						foreach (var neighbor in GetNeighbors(i, j))
						{
							Diff[neighbor.Address] = neighbor;
						}
					}
				}


			foreach (var actionOnProperty in actionOnProperties)
			{
				actionOnProperty.Key.State = actionOnProperty.Value switch
				{
					ActionOnProperty action when action == ActionOnProperty.Build => PropertyState.Populated,
					ActionOnProperty action when action == ActionOnProperty.Destroy => PropertyState.Empty,
					_ => throw new ArgumentOutOfRangeException(nameof(actionOnProperty))
				};
			}

			actionOnProperties.Clear();
		}

		private ActionOnProperty GetActionOnProperty(int x, int y)
		{
			var neighbors = 0;

			// Looking for neighbors
			for (var i = x - 1; i <= x + 1; i++)
			for (var j = y - 1; j <= y + 1; j++)
			{
				if (i == x && j == y)
					continue;

				if (i < 0 || j < 0)
					continue;

				if (i >= _universeParams.Size.Height
				    || j >= _universeParams.Size.Width)
					continue;

				if (Field[i][j].State == PropertyState.Populated)
					neighbors++;
			}

			return neighbors switch
			{
				int n when neighbors != 0 && (n <= 1 || n >= 4) => ActionOnProperty.Destroy,
				int n when n == 3 => ActionOnProperty.Build,
				_ => ActionOnProperty.Nothing
			};
		}

		private IEnumerable<Property> GetNeighbors(int x, int y)
		{
			for (var i = x - 1; i <= x + 1; i++)
			for (var j = y - 1; j <= y + 1; j++)
			{
				if (i == x && j == y)
					continue;

				if (i < 0 || j < 0)
					continue;

				if (i >= _universeParams.Size.Height
				    || j >= _universeParams.Size.Width)
					continue;

				yield return Field[i][j];
			}
		}
	}

	public class UniverseParams
	{
		public UniverseSize Size { get; set; }
		//public List<List<Citizen>> InitialState { get; set; }
	}

	public class UniverseSize
	{
		public int Height { get; set; }
		public int Width { get; set; }
	}

	public enum ActionOnProperty
	{
		Destroy,
		Build,
		Nothing
	}
}