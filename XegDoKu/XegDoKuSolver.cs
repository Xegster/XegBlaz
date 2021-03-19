using System;
using System.Collections.Generic;
using System.Linq;
using XegDoKu.Models;
using XegDoKu.Utilities;

namespace XegDoKu
{
	public class XegDoKuSolver
	{
		public Board Board { get; set; }
		public SolutionState CurrentState { get; set; }
		private Random Generator { get; set; } = new Random();

		public XegDoKuSolver(Board board)
		{
			Board = board;
		}
		public XegDoKuSolver()
		{

		}

		public Solution Solve()
		{
			ReducePossibleValues();
			CurrentState = CheckState();
			if (CurrentState != SolutionState.Processing)
			{
				return new Solution
				{
					State = CurrentState,
					SolvedBoard = Board
				};
			}
			var target = Board.Cells.AsEnumerable().Where(cell => cell.Value == 0).OrderBy(cell => cell.PossibleValues.Count).FirstOrDefault();
			foreach (var pv in target.PossibleValues)
			{
				Board board = new Board(Board);
				var clonedTarget = board.Cells.AsEnumerable().FirstOrDefault(cell => cell.Coordinate == target.Coordinate);
				clonedTarget.Value = pv;
				var newSolver = new XegDoKuSolver(board);
				var result = newSolver.Solve();
				if (result.State == SolutionState.Solved)
					return result;
			}

			return new Solution()
			{
				State = SolutionState.Impossible,
				SolvedBoard = Board
			};
		}

		private SolutionState CheckState()
		{
			if (Board.Cells.ToList().All(cell => cell.Value != 0))
			{
				var result = XegDoKuChecker.VerifyBoard(Board);
				if (result.Count == 0)
					return SolutionState.Solved;
				else return SolutionState.Impossible;
			}
			else if (Board.Cells.ToList().Any(cell => cell.PossibleValues.Count == 0))
				return SolutionState.Impossible;
			else return SolutionState.Processing;
		}

		private int CalculateRandomValue(Cell cell)
		{
			return cell.PossibleValues[Generator.Next(0, cell.PossibleValues.Count)];
		}

		private static List<int> FullList { get { return new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; } }

		public void ReducePossibleValues()
		{
			//Set any cell with 1 possible value
			bool setSingle = SetSinglePossibleValues();

			//now, check each column, row, quadrant - if a possible value only appears in 1 cell in any of these, set it
			bool setSingleOfSet = SetSingleOfSetPossibleValues();

			//now check each quadrant. If a possible value only exists in a single column or row, then it can be removed from all cells in 
			//column/row outside the quadrant
			bool addForbiddenValues = AddForbiddenValues();
			while (setSingle || setSingleOfSet || addForbiddenValues)
			{
				setSingle = SetSinglePossibleValues();
				setSingleOfSet = SetSingleOfSetPossibleValues();
				addForbiddenValues = AddForbiddenValues();

			}
		}

		private bool AddForbiddenValues()
		{
			var ret = false;
			foreach (var quadrant in Board.Quadrants)
			{
				var quadList = quadrant.AsEnumerable().Where(q => q.Value == 0).ToList();
				if (quadList.Count == 0) continue;
				var valueSets = GetValueSets(quadList);
				var applicableRows = valueSets.Where(vs => vs.Cells.All(cell => cell.Coordinate.Row == vs.Cells[0].Coordinate.Row)).ToList();
				var applicableCols = valueSets.Where(vs => vs.Cells.All(cell => cell.Coordinate.Column == vs.Cells[0].Coordinate.Column)).ToList();
				foreach (var row in applicableRows)
				{
					var externalRow = Board.GetRow(row.Cells[0].Coordinate.Row).AsEnumerable().Except(quadList);
					foreach (var cell in externalRow.Where(cell => cell.PossibleValues.Contains(row.Value)))
					{
						cell.ForbiddenValues.Add(row.Value);
						ret = true;
					}
				}
				foreach (var col in applicableCols)
				{
					var externalCol = Board.GetColumn(col.Cells[0].Coordinate.Column).AsEnumerable().Except(quadList);
					foreach (var cell in externalCol.Where(cell => cell.PossibleValues.Contains(col.Value)))
					{
						cell.ForbiddenValues.Add(col.Value);
						ret = true;
					}
				}
			}
			return ret;
		}

		private bool SetSingleOfSetPossibleValues()
		{
			bool ret = false;
			foreach (var quadrant in Board.Quadrants)
			{
				var quadList = quadrant.ToList();
				if (SetSinglePossibleValues(quadList))
					ret = true;
			}
			foreach (var column in Board.Columns)
			{
				var colList = column.ToList();
				if (SetSinglePossibleValues(colList))
					ret = true;
			}
			foreach (var row in Board.Rows)
			{
				var rowList = row.ToList();
				if (SetSinglePossibleValues(rowList))
					ret = true;
			}

			return ret;
		}

		private bool SetSinglePossibleValues()
		{
			var ret = false;
			var onePossible = Board.Cells.AsEnumerable().Where(c => c.Value == 0 && c.PossibleValues.Count == 1).FirstOrDefault();
			while (onePossible != null)
			{
				ret = true;
				onePossible.Value = onePossible.PossibleValues[0];
				onePossible = Board.Cells.AsEnumerable().Where(c => c.Value == 0 && c.PossibleValues.Count == 1).FirstOrDefault();
			}

			return ret;
		}

		private static List<ValueSet> GetValueSets(List<Cell> someList)
		{
			return FullList.Select(fl =>
										new ValueSet
										{
											Value = fl,
											Cells = someList.Where(q => q.PossibleValues.Contains(fl)).ToList()
										}).ToList();
		}

		private bool SetSinglePossibleValues(List<Cell> someList)
		{
			var joined = GetValueSets(someList).Where(j => j.Cells.Count == 1).ToList();
			foreach (var join in joined)
				join.Cells[0].Value = join.Value;
			return joined.Count > 0;
		}
	}
}
