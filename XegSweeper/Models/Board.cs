using System;
using System.Collections.Generic;
using System.Linq;
using XegSweeper.Utilities;

namespace XegSweeper.Models
{
	public class Board
	{

		public Cell[,] Cells { get; set; }
		public int MaxHeight { get { return Cells.GetLength(0); } }
		public int MaxWidth { get { return Cells.GetLength(1); } }
		public GameState CurrentState { get; set; } = GameState.InProgress;
		public Board(Settings settings)
		{
			Cells = new Cell[settings.Height, settings.Width];
			var generator = new Random();
			List<Tuple<int, int>> bombLocations = new List<Tuple<int, int>>();
			for (int i = 0; i < settings.MineCount; i++)
			{
				var randR = generator.Next(0, settings.Height);
				var randC = generator.Next(0, settings.Width);
				while (bombLocations.Any(bl => bl.Item1 == randR && bl.Item2 == randC))
				{
					randR = generator.Next(0, settings.Height);
					randC = generator.Next(0, settings.Width);
				}
				Cells[randR, randC] = new Cell() { Bomb = true };
				bombLocations.Add(new Tuple<int, int>(randR, randC));
			}
			foreach (var bomb in bombLocations)
			{
				MarkNeighbors(Cells, bomb.Item1, bomb.Item2);
			}
			for (int r = 0; r < settings.Height; r++)
			{
				for (int c = 0; c < settings.Width; c++)
				{
					if (Cells[r, c] == null)
						Cells[r, c] = new Cell() { Value = 0 };
				}
			}
		}



		private void MarkNeighbors(Cell[,] cells, int bombRow, int bombColumn)
		{
			for (int r = bombRow - 1; r < bombRow + 2; r++)
			{
				if (r < 0 || r >= MaxHeight)
					continue;
				for (int c = bombColumn - 1; c < bombColumn + 2; c++)
				{
					if (c < 0 || c >= MaxWidth)
						continue;
					if (cells[r, c] == null)
						cells[r, c] = new Cell() { Value = 1 };
					else if (!cells[r, c].Bomb)
						cells[r, c].Value++;
				}
			}
		}

		public void Flag(int row, int column)
		{
			var cell = Cells[row, column];
			if (!cell.Flagged)
				cell.Flagged = true;
			else cell.Flagged = false;
		}

		public void Uncover(int row, int column)
		{
			var cell = Cells[row, column];
			if (cell.Flagged) return;

			cell.Uncovered = true;
			if (cell.Bomb)
				Lose();
			else if (cell.IsEmpty)
				UncoverNeighbors(row, column);
			CheckWin();
		}

		private void CheckWin()
		{
			var allBombsFlagged = AllBombsFlagged();
			var allSpacesOpened = AllSpacesOpened();
			if (allBombsFlagged || allSpacesOpened)
				Win();
		}
		private bool AllSpacesOpened()
		{
			for (int row = 0; row < MaxHeight; row++)
			{
				for (int col = 0; col < MaxWidth; col++)
				{
					if (!Cells[row, col].Bomb && Cells[row, col].Covered)
						return false;
				}
			}
			return true;
		}
		private bool AllBombsFlagged()
		{
			for (int row = 0; row < MaxHeight; row++)
			{
				for (int col = 0; col < MaxWidth; col++)
				{
					if (Cells[row, col].Bomb && !Cells[row, col].Flagged)
						return false;
				}
			}
			return true;
		}

		private void Lose()
		{
			CurrentState = GameState.Lost;
		}
		private void Win()
		{
			CurrentState = GameState.Won;
		}

		private void UncoverNeighbors(int row, int column)
		{
			for (int r = row - 1; r < row + 2; r++)
			{
				if (r < 0 || r >= MaxHeight)
					continue;
				for (int c = column - 1; c < column + 2; c++)
				{
					if (c < 0 || c >= MaxWidth)
						continue;
					if (Cells[r, c].Covered)
						Uncover(r, c);
				}
			}
		}

	}
}
