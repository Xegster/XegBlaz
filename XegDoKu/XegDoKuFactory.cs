using System;
using System.Linq;
using XegDoKu.Models;
using XegDoKu.Utilities;

namespace XegDoKu
{
	public class XegDoKuFactory
	{
		private static Random Generator { get; set; } = new Random();
		public static XegDoKuGame BuildBasicGame()
		{
			var game = new XegDoKuGame();
			Solution solution = new Solution() { State = SolutionState.Processing };
			while (solution.State != SolutionState.Solved)
			{
				var rando = XegDoKuFactory.BuildBase(Settings.EasyStandard);
				game.Board = new Board(Settings.EasyStandard, rando);
				var solvedBoard = new Board(Settings.EasyStandard, rando);
				var solver = new XegDoKuSolver(solvedBoard);
				solution = solver.Solve();
				game.Solution = solution.SolvedBoard;
			}

			return game;
		}

		public static int[,] BuildBase(Settings settings)
		{
			var board = new Board(settings);
			int clueCount = GetRandom(Settings.ClueCount(settings.Difficulty));
			int currentClues = 0;
			while (currentClues < clueCount)
			{
				var targets = board.Cells.AsEnumerable().Where(cell => cell.Value == 0 && cell.PossibleValues.Count > 0).ToList();
				var target = targets.GetRandom();
				target.Value = GetRandomValue(target);
				if (board.Cells.BrokenCells().Count > 0)
				{
					target.ForbiddenValues.Add(target.Value);
					target.Value = 0;
				}
				Strategy.ApplyAllStrategies(board);
				currentClues = board.Cells.AsEnumerable().Where(cell => cell.Value != 0).Count();
			}
			if (board.Cells.BrokenCells().Count > 0)
				return BuildBase(settings);
			else
				return board.Cells.ToValueArray();
		}

		private static int GetRandomValue(Cell cell)
		{
			return cell.PossibleValues.GetRandom();
		}
		private static int GetRandom(Utilities.Range range)
		{
			return Generator.Next(range.Minimum, range.Maximum);
		}
	}
}
