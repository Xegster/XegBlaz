using System;
using System.Collections.Generic;
using XegSweeper.Utilities;

namespace XegSweeper.Models
{
	public class Board
	{

		public Cell[,] Cells { get; private set; }
		public int MaxHeight { get { return Cells.GetLength(0); } }
		public int MaxWidth { get { return Cells.GetLength(1); } }
		public int TotalCells { get { return MaxHeight * MaxWidth; } }
		public int UncoveredCells { get; set; }
		public int MineCount { get; private set; }
		public int FlagCount { get; private set; }
		public int RemainingMines { get { return MineCount - FlagCount; } }
		public GameState CurrentState { get; set; } = GameState.InProgress;
		public int FreeHits { get; set; }

		//private List<Tuple<int, int>> BombLocations { get; set; }

		private Random Generator { get; set; } = new Random();
		public Board(Settings settings)
		{
			//BombLocations = new List<Tuple<int, int>>(); 
			MineCount = settings.MineCount;
			FreeHits = settings.FreeHits;
			Cells = new Cell[settings.Height, settings.Width];
			List<Tuple<int, int>> bombLocations = new List<Tuple<int, int>>();
			for (int i = 0; i < settings.MineCount; i++)
			{
				Tuple<int, int> cellCoords = FindUnusedBombCoords(Cells);
				Cells[cellCoords.Item1, cellCoords.Item2] = new Cell() { Bomb = true };
				bombLocations.Add(cellCoords);
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

		private Tuple<int, int> FindUnusedBombCoords(Cell[,] cells)
		{
			var randR = Generator.Next(0, MaxHeight);
			var randC = Generator.Next(0, MaxWidth);
			var cell = cells[randR, randC];
			while (cell != null && (cell.Bomb || cell.Uncovered))
			{
				randR = Generator.Next(0, MaxHeight);
				randC = Generator.Next(0, MaxWidth);
				cell = cells[randR, randC];
			}
			return new Tuple<int, int>(randR, randC);
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
			{
				cell.Flagged = true;
				FlagCount++;
				CheckWin();
			}
			else
			{
				cell.Flagged = false;
				FlagCount--;
			}
		}

		public void Uncover(int row, int column)
		{

			var cell = Cells[row, column];
			if (cell.Flagged) return;


			cell.Uncovered = true;
			UncoveredCells++;

			if (UncoveredCells <= FreeHits && cell.Bomb)
				MoveBomb(row, column);

			if (cell.Bomb)
				Lose();
			else if (cell.IsEmpty)
				UncoverNeighbors(row, column);
			CheckWin();
		}

		private void MoveBomb(int row, int column)
		{
			var bomb = Cells[row, column];
			bomb.Bomb = false;
			bomb.Value = CalculateClue(row, column);
			var newCoords = FindUnusedBombCoords(Cells);
			var newBomb = Cells[newCoords.Item1, newCoords.Item2];
			newBomb.Value = 0;
			newBomb.Bomb = true;
			MarkNeighbors(Cells, newCoords.Item1, newCoords.Item2);

		}

		private int CalculateClue(int row, int column)
		{
			int ret = 0;
			for (int r = row - 1; r < row + 2; r++)
			{
				if (r < 0 || r >= MaxHeight)
					continue;
				for (int c = column - 1; c < column + 2; c++)
				{
					if (c < 0 || c >= MaxWidth)
						continue;
					if (Cells[r, c].Bomb)
						ret++;
				}
			}
			return ret;
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
