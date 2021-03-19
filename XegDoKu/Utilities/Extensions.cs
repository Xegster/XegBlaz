using System;
using System.Collections.Generic;
using System.Linq;
using XegDoKu.Models;

namespace XegDoKu.Utilities
{
	public static class Extensions
	{
		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		public static IEnumerable<int> GetCellValues(this Cell[,] cells)
		{
			return cells.AsEnumerable().Select(cell => cell.Value);
		}
		public static List<T> ToList<T>(this T[,] cells)
		{
			return cells.Cast<T>().ToList();
		}
		public static IEnumerable<T> AsEnumerable<T>(this T[,] cells)
		{
			return cells.Cast<T>();
		}
		public static IEnumerable<Cell> OrderByPossibleValues(this Cell[,] cells)
		{
			return cells.AsEnumerable().OrderBy(cell => cell.PossibleValues.Count);
		}
		public static List<Cell> BrokenCells(this Cell[,] cells)
		{
			return cells.AsEnumerable().Where(cell => cell.Value == 0 && cell.PossibleValues.Count == 0).ToList();
		}
		public static int[,] ToValueArray(this Cell[,] cells)
		{

			var rows = cells.GetLength(0);
			var cols = cells.GetLength(1);
			var ret = new int[rows, cols];
			for (int r = 0; r < rows; r++)
			{
				for (int c = 0; c < cols; c++)
				{
					ret[r, c] = cells[r, c].Value;
				}
			}
			return ret;
		}
		public static IEnumerable<Cell[]> Rows(this Cell[,] cells)
		{
			var height = cells.GetLength(0);
			for (int i = 0; i < height; i++)
				yield return Utilities.GetRow(cells, i);
		}
		public static IEnumerable<Cell[,]> Quadrants(this Cell[,] cells)
		{
			foreach (var quad in Enum.GetValues(typeof(Quadrant)))
				yield return Utilities.GetQuadrant(cells, (Quadrant)quad);
		}
		public static IEnumerable<Cell[]> Columns(this Cell[,] cells)
		{
			var width = cells.GetLength(1);
			for (int i = 0; i < width; i++)
				yield return Utilities.GetColumn(cells, i);
		}

		//public static void ForEach(this Cell[,] cells, Action<Cell> action)
		//{
		//	for(int r = 0; r < cells.GetLength(0); r++)
		//	{
		//		for(int c = 0; c < cells.GetLength(1); c++)
		//		{
		//			action(cells[r, c]);
		//		}
		//	}
		//}
		//public int MaxHeight { get { return Cells.GetLength(0); } }
		//public int MaxWidth { get { return Cells.GetLength(1); } }


	}
}
