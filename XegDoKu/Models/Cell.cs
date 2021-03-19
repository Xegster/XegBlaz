using System;
using System.Collections.Generic;
using System.Linq;
using XegDoKu.Utilities;

namespace XegDoKu.Models
{
	public class Cell
	{
		public int Value { get; set; }
		public bool Uncovered { get; set; } = false;
		public bool Covered { get { return !Uncovered; } set { Uncovered = !value; } }
		public Coordinate Coordinate { get; set; }
		public Cell[] Column { get; set; }
		public List<int> ColumnValues { get { return Column.Select(col => col.Value).ToList(); } }
		public Cell[] Row { get; set; }
		public List<int> RowValues { get { return Row.Select(row => row.Value).ToList(); } }
		public Cell[,] Quadrant { get; set; }
		public List<int> QuadrantValues { get { return Quadrant.GetCellValues().ToList(); } }

		public List<int> PossibleValues
		{
			get
			{
				return FullList.Except(ColumnValues).Except(RowValues).Except(QuadrantValues).Except(ForbiddenValues).ToList();
			}
		}
		public List<int> ForbiddenValues { get; set; } = new List<int>();
		public override string ToString()
		{
			return string.Format("{0} - {1}", Coordinate, Value);
		}
		private List<int> FullList { get { return new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; } }

	}
}
