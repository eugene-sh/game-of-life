using System.Collections.Generic;
using System.Linq;
using GameOfLife.Model;

namespace GameOfLife.Tests
{
	public class UniverseParamsBuilder
	{
		private readonly UniverseParams _universeParams;

		private readonly List<Cell> _cells = new List<Cell>();

		public UniverseParamsBuilder()
		{
			_universeParams = new UniverseParams();
		}

		public UniverseParamsBuilder WithSize(UniverseSize universeSize)
		{
			_universeParams.Size = universeSize;
			return this;
		}

		public UniverseParamsBuilder WithField(List<List<Cell>> field)
		{
			_universeParams.Field = field;
			return this;
		}

		public UniverseParamsBuilder WithCell(Cell cell)
		{
			_cells.Add(cell);
			return this;
		}

		public UniverseParams Build()
		{
			const int defaultHeight = 50;
			const int defaultWith = 50;

			if (_universeParams.Size is null)
				_universeParams.Size = new UniverseSize(defaultHeight, defaultWith);
			
			if (!_universeParams.Field.Any())
				_universeParams.Field = GenerateDefaultField(defaultHeight, defaultWith);

			return _universeParams;
		}

		private List<List<Cell>> GenerateDefaultField(int height, int width)
		{
			var field = new List<List<Cell>>();
			for (var i = 0; i < height; i++)
			{
				field.Add(new List<Cell>());

				for (var j = 0; j < width; j++)
				{
					var requestedCell = _cells.FirstOrDefault(p => p.Position.X == i && p.Position.Y == j);
					if (requestedCell != null)
					{
						field[i].Add(requestedCell);
						continue;
					}

					field[i].Add(new Cell
					{
						State = CellState.Unpopulated,
						Position = new CellPosition(i, j)
					});
				}

			}

			return field;
		}
	}
}