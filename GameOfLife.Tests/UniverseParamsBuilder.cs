using System.Collections.Generic;
using System.Linq;
using GameOfLife.Model;

namespace GameOfLife.Tests
{
	public class UniverseParamsBuilder
	{
		private readonly UniverseParams _universeParams;

		private readonly List<Property> _properties = new List<Property>();

		public UniverseParamsBuilder()
		{
			_universeParams = new UniverseParams();
		}

		public UniverseParamsBuilder WithSize(UniverseSize universeSize)
		{
			_universeParams.Size = universeSize;
			return this;
		}

		public UniverseParamsBuilder WithField(List<List<Property>> field)
		{
			_universeParams.Field = field;
			return this;
		}

		public UniverseParamsBuilder WithProperty(Property property)
		{
			_properties.Add(property);
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

		private List<List<Property>> GenerateDefaultField(int height, int width)
		{
			var field = new List<List<Property>>();
			for (var i = 0; i < height; i++)
			{
				field.Add(new List<Property>());

				for (var j = 0; j < width; j++)
				{
					var requestedProperty = _properties.FirstOrDefault(p => p.Address.X == i && p.Address.Y == j);
					if (requestedProperty != null)
					{
						field[i].Add(requestedProperty);
						continue;
					}

					field[i].Add(new Property
					{
						State = PropertyState.Empty,
						Address = new PropertyAddress(i, j)
					});
				}

			}

			return field;
		}
	}
}