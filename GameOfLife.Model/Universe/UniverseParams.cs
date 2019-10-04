using System.Collections.Generic;

namespace GameOfLife.Model
{
	public class UniverseParams
	{
		public UniverseSize Size { get; set; }

		public List<List<Cell>> Field { get; set; } = new List<List<Cell>>();
	}


}