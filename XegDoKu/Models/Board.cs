using System;
using System.Collections.Generic;
using System.Linq;
using XegDoKu.Utilities;
using Utils = XegDoKu.Utilities.Utilities;
namespace XegDoKu.Models
{
	public class Board
	{
		public Cell[,] Cells { get; set; }
		public Settings CurrentSettings { get; set; }
		public int Height { get { return (int)CurrentSettings.Size; } }
		public int Width { get { return (int)CurrentSettings.Size; } }
		private Random Generator { get; set; } = new Random();
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
		private List<int> FullList { get { return new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; } }

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
			//Cells.Cast<Cell>().ToList().ForEach(cell => cell = new Cell());

			//SetValues(Cells);

			//var quads = GetQuadrant(Quadrant.UpperLeft).ToList().Union(GetQuadrant(Quadrant.MiddleMiddle).ToList()).Union(GetQuadrant(Quadrant.LowerRight).ToList());
			//var target = quads.AsEnumerable().Where(cell => cell.Value == 0).OrderBy(cell => cell.PossibleValues.Count).FirstOrDefault();
			//while (target != null)
			//{
			//	target.Value = CalculateRandomValue(target);
			//	target = quads.AsEnumerable().Where(cell => cell.Value == 0).OrderBy(cell => cell.PossibleValues.Count).FirstOrDefault();
			//}

			//target = Cells.AsEnumerable().Where(cell => cell.Value == 0).OrderBy(cell => cell.PossibleValues.Count).FirstOrDefault();
			//var possible = Cells.AsEnumerable().Where(cell => cell.Value == 0).Where(cell => cell.PossibleValues.Count > 0).ToList();
			//while (target != null && possible.Count > 0)
			//{
			//	target.Value = CalculateRandomValue(target);
			//	target = Cells.AsEnumerable().Where(cell => cell.Value == 0).OrderBy(cell => cell.PossibleValues.Count).FirstOrDefault();
			//	possible = Cells.AsEnumerable().Where(cell => cell.Value == 0).Where(cell => cell.PossibleValues.Count > 0).ToList();
			//}
			//for (int i = 0; i < (Width * Height); i++)
			//{
			//	target = Cells.AsEnumerable().Where(cell => cell.Value == 0).OrderBy(cell => cell.PossibleValues.Count).FirstOrDefault();
			//	target.Value = CalculateRandomValue(target);
			//}

			//foreach (var quadrant in Quadrants)
			//{
			//	foreach (var cell in quadrant.AsEnumerable().OrderBy(c => c.PossibleValues.Count).ToList())
			//	{
			//		if (cell.Value == 0)
			//			cell.Value = CalculateRandomValue(cell);
			//	}
			//}
			//Cells.ToList().ForEach(cell =>
			//{
			//	cell.Value = CalculateRandomValue(cell);
			//});
		}

		private void SetValues(Cell[,] cells)
		{
			var target = cells.AsEnumerable().Where(cell => cell.Value == 0).OrderBy(cell => cell.PossibleValues.Count).FirstOrDefault();
			while (target != null && target.PossibleValues.Count > 0)
			{
				target.Value = CalculateRandomValue(target);
				while (cells.BrokenCells().Count > 0 && target.PossibleValues.Count > 0)
				{
					target.ForbiddenValues.Add(target.Value);
					target.Value = 0;
					target.Value = CalculateRandomValue(target);
				}
				target = cells.AsEnumerable().Where(cell => cell.Value == 0).OrderBy(cell => cell.PossibleValues.Count).FirstOrDefault();
			}
		}

		private int CalculateRandomValue(Cell cell)
		{
			return cell.PossibleValues[Generator.Next(0, cell.PossibleValues.Count)];
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

	}
}
