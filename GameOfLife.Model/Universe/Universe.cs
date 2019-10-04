using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Model
{
	public sealed class Universe
	{
		public List<List<Property>> Field { get; } = new List<List<Property>>();
		public readonly Dictionary<PropertyAddress, Property> TrackList = new Dictionary<PropertyAddress, Property>();

		private readonly UniverseParams _universeParams;

		public Universe(UniverseParams universeParams)
		{
			_universeParams = universeParams ?? throw new ArgumentNullException(nameof(universeParams));

			Field = _universeParams.Field;

			for (var i = 0; i < _universeParams.Size.Height; i++)
			for (var j = 0; j < _universeParams.Size.Width; j++)
			{
				var actionOnProperty = GetActionOnProperty(Field[i][j]);
				if (actionOnProperty == ActionOnProperty.Nothing)
					continue;

				foreach (var neighbor in GetNeighbors(new PropertyAddress(i, j)))
					TrackList[neighbor.Address] = neighbor;
			}
		}

		public void NextGeneration()
		{
			var propertiesForAddingToTrackList = new Dictionary<PropertyAddress, Property>();
			var propertiesForRemovingFromTrackList = new Dictionary<PropertyAddress, Property>();
			var actionOnTrackedProperties = new Dictionary<Property, ActionOnProperty>();

			foreach (var (address, property) in TrackList)
			{
				var actionOnProperty = GetActionOnProperty(property);
				if (actionOnProperty == ActionOnProperty.Nothing)
				{
					propertiesForRemovingFromTrackList.Add(address, property);
					continue;
				}

				actionOnTrackedProperties[Field[address.X][address.Y]] = actionOnProperty;

				foreach (var neighbor in GetNeighbors(address))
					propertiesForAddingToTrackList[neighbor.Address] = neighbor;
			}

			foreach (var item in propertiesForRemovingFromTrackList)
				TrackList.Remove(item.Key);
			propertiesForRemovingFromTrackList.Clear();

			foreach (var (propertyAddress, property) in propertiesForAddingToTrackList)
					TrackList[propertyAddress] = property;
			propertiesForAddingToTrackList.Clear();

			foreach (var actionOnProperty in actionOnTrackedProperties)
			{
				actionOnProperty.Key.State = actionOnProperty.Value switch
				{
					ActionOnProperty action when action == ActionOnProperty.Build => PropertyState.Populated,
					ActionOnProperty action when action == ActionOnProperty.Destroy => PropertyState.Empty,
					_ => throw new ArgumentOutOfRangeException(nameof(actionOnProperty))
				};
			}
			actionOnTrackedProperties.Clear();
		}

		private ActionOnProperty GetActionOnProperty(Property property)
		{
			var neighbors = GetNeighbors(property.Address)
				.Count(n => n.State == PropertyState.Populated);


			return neighbors switch
			{
				// TODO: 0_0
				int n when n == 0 && property.State == PropertyState.Empty => ActionOnProperty.Nothing,
				int n when property.State == PropertyState.Populated && n == 1 || n >= 4 || n == 0 => ActionOnProperty.Destroy,
				int n when property.State == PropertyState.Empty && n == 3 => ActionOnProperty.Build,
				_ => ActionOnProperty.Nothing
			};
		}

		private IEnumerable<Property> GetNeighbors(PropertyAddress propertyAddress)
		{
			var x = propertyAddress.X;
			var y = propertyAddress.Y;

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