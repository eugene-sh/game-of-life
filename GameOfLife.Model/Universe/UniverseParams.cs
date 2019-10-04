using System.Collections.Generic;

namespace GameOfLife.Model
{
	public class UniverseParams
	{
		public UniverseSize Size { get; set; }

		public List<List<Property>> Field { get; set; } = new List<List<Property>>();
	}


}