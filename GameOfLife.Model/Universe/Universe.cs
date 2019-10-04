using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Model
{
	public sealed class Universe
	{
		public List<List<Cell>> Field { get; }
		public readonly Dictionary<CellPosition, Cell> TrackCells = new Dictionary<CellPosition, Cell>();

		private readonly UniverseParams _universeParams;

		public Universe(UniverseParams universeParams)
		{
			_universeParams = universeParams ?? throw new ArgumentNullException(nameof(universeParams));

			Field = _universeParams.Field;

			for (var i = 0; i < _universeParams.Size.Height; i++)
			for (var j = 0; j < _universeParams.Size.Width; j++)
			{
				var actionOnCell = GetActionOnCell(Field[i][j]);
				if (actionOnCell == ActionOnCell.Nothing)
					continue;

				foreach (var neighbor in GetNeighbors(new CellPosition(i, j)))
					TrackCells[neighbor.Position] = neighbor;
			}
		}

		public void NextGeneration()
		{
			var cellsForAddingToTrackList = new Dictionary<CellPosition, Cell>();
			var cellsForRemovingFromTrackList = new Dictionary<CellPosition, Cell>();
			var actionOnTrackedCells = new Dictionary<Cell, ActionOnCell>();

			foreach (var (address, cell) in TrackCells)
			{
				var actionOnCell = GetActionOnCell(cell);
				if (actionOnCell == ActionOnCell.Nothing)
				{
					cellsForRemovingFromTrackList.Add(address, cell);
					continue;
				}

				actionOnTrackedCells[Field[address.X][address.Y]] = actionOnCell;

				foreach (var neighbor in GetNeighbors(address))
					cellsForAddingToTrackList[neighbor.Position] = neighbor;
			}

			foreach (var item in cellsForRemovingFromTrackList)
				TrackCells.Remove(item.Key);
			cellsForRemovingFromTrackList.Clear();

			foreach (var (cellPosition, cell) in cellsForAddingToTrackList)
					TrackCells[cellPosition] = cell;
			cellsForAddingToTrackList.Clear();

			foreach (var actionOnCell in actionOnTrackedCells)
			{
				actionOnCell.Key.State = actionOnCell.Value switch
				{
					ActionOnCell action when action == ActionOnCell.Populate => CellState.Populated,
					ActionOnCell action when action == ActionOnCell.Destroy => CellState.Unpopulated,
					_ => throw new ArgumentOutOfRangeException(nameof(actionOnCell))
				};
			}
			actionOnTrackedCells.Clear();
		}

		private ActionOnCell GetActionOnCell(Cell cell)
		{
			var neighbors = GetNeighbors(cell.Position)
				.Count(n => n.State == CellState.Populated);


			return neighbors switch
			{
				// TODO: 0_0
				int n when n == 0 && cell.State == CellState.Unpopulated => ActionOnCell.Nothing,
				int n when cell.State == CellState.Populated && n == 1 || n >= 4 || n == 0 => ActionOnCell.Destroy,
				int n when cell.State == CellState.Unpopulated && n == 3 => ActionOnCell.Populate,
				_ => ActionOnCell.Nothing
			};
		}

		private IEnumerable<Cell> GetNeighbors(CellPosition cellPosition)
		{
			var x = cellPosition.X;
			var y = cellPosition.Y;

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
}