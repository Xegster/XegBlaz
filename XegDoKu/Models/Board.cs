using System;
using System.Collections.Generic;
using XegDoKu.Utilities;
using Utils = XegDoKu.Utilities.Utilities;
namespace XegDoKu.Models
{
	public class Board
	{
		#region Properties
		public Cell[,] Cells { get; set; }
		public Settings CurrentSettings { get; set; }
		public int Height { get { return (int)CurrentSettings.Size; } }
		public int Width { get { return (int)CurrentSettings.Size; } }
		public IEnumerable<Cell[,]> Quadrants
		{
			get
			{
				return Cells.Quadrants();
			}
		}
		public IEnumerable<Cell[]> Columns
		{
			get
			{
				return Cells.Columns();
			}
		}
		public IEnumerable<Cell[]> Rows
		{
			get
			{
				return Cells.Rows();

			}
		}
		#endregion
		#region Fields
		private List<int> FullList { get { return new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; } }
		private Random Generator { get; set; } = new Random();
		#endregion
		#region Constructors
		public Board(Board board)
		{
			CurrentSettings = board.CurrentSettings;
			Cells = Utils.BuildCells(CurrentSettings.Size);
			Utils.CopyCells(board.Cells, Cells, CurrentSettings.Size);
		}

		public Board(Settings settings, int[,] board)
		{
			CurrentSettings = settings;
			Cells = Utils.BuildCells(CurrentSettings.Size);
			Utils.CopyCells(board, Cells, CurrentSettings.Size);

		}

		public Board(Settings settings, List<CellDTO> cellsToLoad)
		{
			CurrentSettings = settings;
			Cells = Utils.BuildCells(CurrentSettings.Size);
			Utils.CopyCells(cellsToLoad, Cells, CurrentSettings.Size);

		}


		public Board(Settings settings)
		{
			CurrentSettings = settings;
			Cells = Utils.BuildCells(settings.Size);
		}
		#endregion

		#region Helpers
		public List<CellDTO> CompareCellValues(Board board)
		{
			List<CellDTO> mismatchedCells = new List<CellDTO>();
			for (int r = 0; r < Height; r++)
			{
				for (int c = 0; c < Width; c++)
				{
					if (Cells[r, c].Value != board.Cells[r, c].Value)
						mismatchedCells.Add(new CellDTO { Row = r, Column = c });
				}
			}
			return mismatchedCells;
		}
		public Cell[] GetColumn(int column)
		{
			return Utils.GetColumn(Cells, column);
		}
		public Cell[] GetRow(int row)
		{
			return Utils.GetRow(Cells, row);
		}
		private Cell[,] GetQuadrant(int row, int column)
		{
			return Utils.GetQuadrant(Cells, row, column);
		}
		private Cell[,] GetQuadrant(Quadrant quad)
		{
			return Utils.GetQuadrant(Cells, quad);
		}
		#endregion
	}
}
