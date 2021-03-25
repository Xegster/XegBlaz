using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XegDoKu.Interfaces;
using XegDoKu.Models;
using XegDoKu.Utilities;

namespace XegDoKu
{
	public class XegDoKuSolver
	{
		#region Properties
		public Board Board { get; set; }
		public SolutionState CurrentState { get; set; }
		private Random Generator { get; set; } = new Random();
		private ISolverNotify SolverNotify { get; set; }
		#endregion

		#region Fields
		private static List<int> FullList { get { return new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; } }
		#endregion

		#region Constructors
		public XegDoKuSolver(Board board, ISolverNotify solverNotify)
		{
			Board = board;
			SolverNotify = solverNotify;
		}
		public XegDoKuSolver(Board board) : this(board, new SolverNotify())
		{

		}
		#endregion

		public Solution Solve()
		{
			var modified = ApplyAllStrategies();
			if (modified)
				SolverNotify.BoardModified(Board);
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
				SolverNotify.BoardAdded(board);
				var clonedTarget = board.Cells.AsEnumerable().FirstOrDefault(cell => cell.Coordinate == target.Coordinate);
				clonedTarget.Value = pv;
				var newSolver = new XegDoKuSolver(board, SolverNotify);
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

		public Task<List<Solution>> SolveAsync()
		{
			return Task.Run(() =>
			{
				var modified = ApplyAllStrategies();
				if (modified)
					SolverNotify.BoardModified(Board);
				CurrentState = CheckState();
				var ret = new List<Solution>();
				if (CurrentState != SolutionState.Processing)
				{
					ret.Add(new Solution
					{
						State = CurrentState,
						SolvedBoard = Board
					});
					return ret;
				}
				List<Task<List<Solution>>> tasks = new List<Task<List<Solution>>>();
				var target = Board.Cells.AsEnumerable().EmptyCells().OrderBy(cell => cell.Coordinate.Row).ThenBy(cell => cell.Coordinate.Column).First();
				//foreach (var empty in empties)
				//{
					foreach (var pv in target.PossibleValues)
					{
						Board board = new Board(Board);
						SolverNotify.BoardAdded(board);
						var clonedTarget = board.Cells.AsEnumerable().FirstOrDefault(cell => cell.Coordinate == target.Coordinate);
						clonedTarget.Value = pv;
						var newSolver = new XegDoKuSolver(board, SolverNotify);
						tasks.Add(newSolver.SolveAsync());
					}
				//}
				while (tasks.Any(t => !t.IsCompleted))
				{
					var task = tasks.FirstOrDefault(t => !t.IsCompleted);
					if (task != null)
						task.Wait();
				}
				//ret.AddRange(tasks.SelectMany(t => t.Result).Where(s => s.State == SolutionState.Solved));

				var allSolutions = tasks.SelectMany(t => t.Result).Where(s => s.State == SolutionState.Solved).ToList();
				
				foreach (var solution in allSolutions)
				{
					if (!ret.Any(sol => sol.SolvedBoard.CompareCellValues(solution.SolvedBoard).Count == 0))
					{
						ret.Add(solution);
					}
				}
				return ret;
			});
		}

		private SolutionState CheckState()
		{
			if (Board.Cells.AsEnumerable().EmptyCells().Count() == 0)
			{
				var result = XegDoKuChecker.VerifyBoard(Board);
				if (result.Count == 0)
					return SolutionState.Solved;
				else return SolutionState.Impossible;
			}
			else if (Board.Cells.AsEnumerable().EmptyCells().Any(cell => cell.PossibleValues.Count == 0))
				return SolutionState.Impossible;
			else return SolutionState.Processing;
		}


		#region Strategies
		public bool ApplyAllStrategies()
		{
			return Strategy.ApplyAllStrategies(Board);
		}
		/// <summary>
		/// Box line reduction strategy – A form of intersection removal in which candidates which must belong to a line 
		/// can be ruled out as candidates in a block (or box) that intersects the line in question.
		/// </summary>
		/// <returns></returns>
		private bool BoxLineReductionStrategy()
		{
			return Strategy.BoxLineReductionStrategy(Board);
		}
		/// <summary>
		/// Cross hatching – Process of elimination that checks rows and columns intersecting a block for a 
		/// given value to limit the possible locations in the block.
		/// </summary>
		/// <returns></returns>
		private bool CrossHatch()
		{
			return Strategy.CrossHatch(Board);
		}

		private bool SetSinglePossibleValues()
		{
			return Strategy.SetSinglePossibleValues(Board);
		}

		private bool SetSinglePossibleValues(List<Cell> someList)
		{
			return Strategy.SetSinglePossibleValues(someList);
		}
		#endregion

		#region Helpers

		private int CalculateRandomValue(Cell cell)
		{
			return cell.PossibleValues[Generator.Next(0, cell.PossibleValues.Count)];
		}

		#endregion
	}
}
