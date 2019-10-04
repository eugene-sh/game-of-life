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

			universe.TrackList.ShouldBeEmpty();
		}

		[Test]
		public void LonelyPropertyWillBeRemove()
		{
			/*
			 * 0 0 0 0
			 * 0 1 0 0
			 * 0 0 0 0
			 * 0 0 0 0
			 */
			var universeParams = new UniverseParamsBuilder()
				.WithProperty(new Property
				{
					Address = new PropertyAddress(1, 1),
					State = PropertyState.Populated
				})
				.Build();
			var universe = new Universe(universeParams);

			universe.TrackList.ShouldNotBeEmpty();
			universe.TrackList.Count.ShouldBe(8);
			
			universe.NextGeneration();

			universe.TrackList.ShouldBeEmpty();
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
				.WithProperty(new Property
				{
					Address = new PropertyAddress(1, 1),
					State = PropertyState.Populated
				})
				.WithProperty(new Property
				{
					Address = new PropertyAddress(1, 2),
					State = PropertyState.Populated
				})
				.WithProperty(new Property
				{
					Address = new PropertyAddress(2, 1),
					State = PropertyState.Populated
				})
				.WithProperty(new Property
				{
					Address = new PropertyAddress(2, 2),
					State = PropertyState.Populated
				})
				.Build();

			var universe = new Universe(universeParams);

			universe.TrackList.ShouldBeEmpty();
			universe.TrackList.Count.ShouldBe(0);
			
			universe.NextGeneration();

			universe.TrackList.ShouldBeEmpty();
			universe.TrackList.Count.ShouldBe(0);
		}
		
		[Test]
		public void BuildNewPropertyWhenThreeNeighbors()
		{
			/*
			 * 0 0 0 0
			 * 0 1 1 0
			 * 0 1 0 0
			 * 0 0 0 0
			 */
			var universeParams = new UniverseParamsBuilder()
				.WithProperty(new Property
				{
					Address = new PropertyAddress(1, 1),
					State = PropertyState.Populated
				})
				.WithProperty(new Property
				{
					Address = new PropertyAddress(1, 2),
					State = PropertyState.Populated
				})
				.WithProperty(new Property
				{
					Address = new PropertyAddress(2, 1),
					State = PropertyState.Populated
				})
				.Build();

			var universe = new Universe(universeParams);

			universe.TrackList.ShouldNotBeEmpty();
			universe.TrackList.Count.ShouldBe(8);
			
			universe.NextGeneration();

			universe.TrackList.ShouldBeEmpty();
			universe.TrackList.Count.ShouldBe(0);
		}
		
		[Test]
		public void DestroyPropertyWhenMoreThanThreeNeighbors()
		{
			/*
			 * 0 0 0 0 0
			 * 0 0 1 0 0
			 * 0 1 0 1 0
			 * 0 0 1 0 0
			 * 0 0 0 0 0
			 */
			var universeParams = new UniverseParamsBuilder()
				.WithProperty(new Property
				{
					Address = new PropertyAddress(2, 1),
					State = PropertyState.Populated
				})
				.WithProperty(new Property
				{
					Address = new PropertyAddress(2, 3),
					State = PropertyState.Populated
				})
				.WithProperty(new Property
				{
					Address = new PropertyAddress(1, 2),
					State = PropertyState.Populated
				})
				.WithProperty(new Property
				{
					Address = new PropertyAddress(3, 2),
					State = PropertyState.Populated
				})
				.WithProperty(new Property
				{
					Address = new PropertyAddress(2, 2),
					State = PropertyState.Populated
				})
				.Build();

			var universe = new Universe(universeParams);

			universe.Field[2][2].State.ShouldBe(PropertyState.Populated);

			universe.NextGeneration();

			universe.Field[2][2].State.ShouldBe(PropertyState.Empty);
		}
	}
}