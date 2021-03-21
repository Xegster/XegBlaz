using System.Collections.Generic;
using System.Linq;
using XegDoKu.Models;
using XegDoKu.Utilities;

namespace XegDoKu
{
	public class XegDoKuChecker
	{
		public static List<Cell> VerifyBoard(Board board)
		{
			List<Cell> errors = new List<Cell>();
			for (int i = 0; i < board.Width; i++)
			{
				var col = board.GetColumn(i);
				var row = board.GetRow(i);
				errors.AddRange(VerifyCells(col));
				errors.AddRange(VerifyCells(row));
			}
			foreach (var quadrant in board.Quadrants)
			{
				errors.AddRange(VerifyQuadrant(quadrant));
			}
			foreach (var cell in board.Cells.AsEnumerable().Where(c => c.Value == 0))
			{
				if (!errors.Contains(cell))
					errors.Add(cell);
			}
			return errors;
		}

		private static List<Cell> VerifyQuadrant(Cell[,] quadrant)
		{
			return quadrant.AsEnumerable().GroupBy(cell => cell.Value).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
		}

		private static List<Cell> VerifyCells(Cell[] cells)
		{
			return cells.GroupBy(cell => cell.Value).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
		}

		private static IEnumerable<int> FullList { get { return new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; } }


	}
}
