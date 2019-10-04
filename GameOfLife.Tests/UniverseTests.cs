using System;
using GameOfLife.Model;
using NUnit.Framework;
using Shouldly;

namespace GameOfLife.Tests
{
	public class UniverseTests
	{
		[Test]
		public void ArgumentNullExceptionWhenArgumentIsNull()
		{
			Should.Throw<ArgumentNullException>(() => new Universe(null));
		}

		[Test]
		public void WhenEmptyFieldNothingHappen()
		{
			var universeParams = new UniverseParamsBuilder().Build();
			var universe = new Universe(universeParams);

			universe.NextGeneration();

			universe.TrackCells.ShouldBeEmpty();
		}

		[Test]
		public void LonelyCellWillBeRemove()
		{
			/*
			 * 0 0 0 0
			 * 0 1 0 0
			 * 0 0 0 0
			 * 0 0 0 0
			 */
			var universeParams = new UniverseParamsBuilder()
				.WithCell(new Cell
				{
					Position = new CellPosition(1, 1),
					State = CellState.Populated
				})
				.Build();
			var universe = new Universe(universeParams);

			universe.TrackCells.ShouldNotBeEmpty();
			universe.TrackCells.Count.ShouldBe(8);
			
			universe.NextGeneration();

			universe.TrackCells.ShouldBeEmpty();
		}
		
		[Test]
		public void BlockNeverDie()
		{
			/*
			 * 0 0 0 0
			 * 0 1 1 0
			 * 0 1 1 0
			 * 0 0 0 0
			 */
			var universeParams = new UniverseParamsBuilder()
				.WithCell(new Cell
				{
					Position = new CellPosition(1, 1),
					State = CellState.Populated
				})
				.WithCell(new Cell
				{
					Position = new CellPosition(1, 2),
					State = CellState.Populated
				})
				.WithCell(new Cell
				{
					Position = new CellPosition(2, 1),
					State = CellState.Populated
				})
				.WithCell(new Cell
				{
					Position = new CellPosition(2, 2),
					State = CellState.Populated
				})
				.Build();

			var universe = new Universe(universeParams);

			universe.TrackCells.ShouldBeEmpty();
			universe.TrackCells.Count.ShouldBe(0);
			
			universe.NextGeneration();

			universe.TrackCells.ShouldBeEmpty();
			universe.TrackCells.Count.ShouldBe(0);
		}
		
		[Test]
		public void PopulateNewCellWhenThreeNeighbors()
		{
			/*
			 * 0 0 0 0
			 * 0 1 1 0
			 * 0 1 0 0
			 * 0 0 0 0
			 */
			var universeParams = new UniverseParamsBuilder()
				.WithCell(new Cell
				{
					Position = new CellPosition(1, 1),
					State = CellState.Populated
				})
				.WithCell(new Cell
				{
					Position = new CellPosition(1, 2),
					State = CellState.Populated
				})
				.WithCell(new Cell
				{
					Position = new CellPosition(2, 1),
					State = CellState.Populated
				})
				.Build();

			var universe = new Universe(universeParams);

			universe.TrackCells.ShouldNotBeEmpty();
			universe.TrackCells.Count.ShouldBe(8);
			
			universe.NextGeneration();

			universe.TrackCells.ShouldBeEmpty();
			universe.TrackCells.Count.ShouldBe(0);
		}
		
		[Test]
		public void DestroyCellWhenMoreThanThreeNeighbors()
		{
			/*
			 * 0 0 0 0 0
			 * 0 0 1 0 0
			 * 0 1 0 1 0
			 * 0 0 1 0 0
			 * 0 0 0 0 0
			 */
			var universeParams = new UniverseParamsBuilder()
				.WithCell(new Cell
				{
					Position = new CellPosition(2, 1),
					State = CellState.Populated
				})
				.WithCell(new Cell
				{
					Position = new CellPosition(2, 3),
					State = CellState.Populated
				})
				.WithCell(new Cell
				{
					Position = new CellPosition(1, 2),
					State = CellState.Populated
				})
				.WithCell(new Cell
				{
					Position = new CellPosition(3, 2),
					State = CellState.Populated
				})
				.WithCell(new Cell
				{
					Position = new CellPosition(2, 2),
					State = CellState.Populated
				})
				.Build();

			var universe = new Universe(universeParams);

			universe.Field[2][2].State.ShouldBe(CellState.Populated);

			universe.NextGeneration();

			universe.Field[2][2].State.ShouldBe(CellState.Unpopulated);
		}
	}
}