using System.Collections.Generic;
using System.Linq;
using XegDoKu.Models;

namespace XegDoKu.Utilities
{
	public static class Strategy
	{
		public static bool ApplyAllStrategies(Board board)
		{
			bool modified = false;
			//Set any cell with 1 possible value
			bool setSingle = SetSinglePossibleValues(board);

			//now, check each column, row, quadrant - if a possible value only appears in 1 cell in any of these, set it
			bool setSingleOfSet = CrossHatch(board);

			//now check each quadrant. If a possible value only exists in a single column or row, then it can be removed from all cells in 
			//column/row outside the quadrant
			bool addForbiddenValues = BoxLineReductionStrategy(board);
			while (setSingle || setSingleOfSet || addForbiddenValues)
			{
				modified = true;
				setSingle = SetSinglePossibleValues(board);
				setSingleOfSet = CrossHatch(board);
				addForbiddenValues = BoxLineReductionStrategy(board);
			}
			return modified;
		}
		/// <summary>
		/// Box line reduction strategy – A form of intersection removal in which candidates which must belong to a line 
		/// can be ruled out as candidates in a block (or box) that intersects the line in question.
		/// </summary>
		/// <returns></returns>

		public static bool BoxLineReductionStrategy(Board board)
		{
			var ret = false;
			foreach (var quadrant in board.Quadrants)
			{
				var quadList = quadrant.AsEnumerable().EmptyCells().ToList();
				if (quadList.Count == 0) continue;
				var valueSets = GetValueSets(quadList);
				var applicableRows = valueSets.Where(vs => vs.Cells.Any() && vs.Cells.All(cell => cell.Coordinate.Row == vs.Cells[0].Coordinate.Row)).ToList();
				var applicableCols = valueSets.Where(vs => vs.Cells.Any() && vs.Cells.All(cell => cell.Coordinate.Column == vs.Cells[0].Coordinate.Column)).ToList();
				foreach (var row in applicableRows)
				{
					var externalRow = board.GetRow(row.Cells[0].Coordinate.Row).AsEnumerable().Except(quadList);
					foreach (var cell in externalRow.Where(cell => cell.PossibleValues.Contains(row.Value)))
					{
						cell.ForbiddenValues.Add(row.Value);
						ret = true;
					}
				}
				foreach (var col in applicableCols)
				{
					var externalCol = board.GetColumn(col.Cells[0].Coordinate.Column).AsEnumerable().Except(quadList);
					foreach (var cell in externalCol.Where(cell => cell.PossibleValues.Contains(col.Value)))
					{
						cell.ForbiddenValues.Add(col.Value);
						ret = true;
					}
				}
			}
			return ret;
		}
		/// <summary>
		/// Cross hatching – Process of elimination that checks rows and columns intersecting a block for a 
		/// given value to limit the possible locations in the block.
		/// </summary>
		/// <returns></returns>
		public static bool CrossHatch(Board board)
		{
			bool ret = false;
			foreach (var quadrant in board.Quadrants)
			{
				var quadList = quadrant.ToList();
				if (SetSinglePossibleValues(quadList))
					ret = true;
			}
			foreach (var column in board.Columns)
			{
				var colList = column.ToList();
				if (SetSinglePossibleValues(colList))
					ret = true;
			}
			foreach (var row in board.Rows)
			{
				var rowList = row.ToList();
				if (SetSinglePossibleValues(rowList))
					ret = true;
			}

			return ret;
		}

		public static bool SetSinglePossibleValues(Board board)
		{
			var ret = false;
			var onePossible = board.Cells.AsEnumerable().Where(c => c.Value == 0 && c.PossibleValues.Count == 1).FirstOrDefault();
			while (onePossible != null)
			{
				ret = true;
				onePossible.Value = onePossible.PossibleValues[0];
				onePossible = board.Cells.AsEnumerable().Where(c => c.Value == 0 && c.PossibleValues.Count == 1).FirstOrDefault();
			}

			return ret;
		}

		public static bool SetSinglePossibleValues(List<Cell> someList)
		{
			var joined = GetValueSets(someList).Where(j => j.Cells.Count == 1).ToList();
			var ret = false;
			foreach (var join in joined)
			{
				if (join.Cells[0].Value == 0)
				{
					ret = true;
					join.Cells[0].Value = join.Value;
				}
			}
			return ret;
		}
		private static List<int> FullList { get { return new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; } }
		public static List<ValueSet> GetValueSets(List<Cell> someList)
		{
			return FullList.Select(fl =>
										new ValueSet
										{
											Value = fl,
											Cells = someList.Where(q => q.Value == 0 && q.PossibleValues.Contains(fl)).ToList()
										}).ToList();
		}
	}
}
