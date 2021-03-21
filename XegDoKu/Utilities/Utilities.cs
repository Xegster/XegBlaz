using System;
using System.Collections.Generic;
using System.Linq;
using XegDoKu.Models;

namespace XegDoKu.Utilities
{
	public class Utilities
	{
		public static void CopyCells(Cell[,] source, Cell[,] target, SudokuSize sSize)
		{
			int size = (int)sSize;
			for (int row = 0; row < size; row++)
			{
				for (int col = 0; col < size; col++)
				{
					target[row, col].Value = source[row, col].Value;
					target[row, col].ForbiddenValues = source[row, col].ForbiddenValues.Select(fv => fv).ToList();

				}
			}

		}
		public static void CopyCells(int[,] source, Cell[,] target, SudokuSize sSize)
		{
			int size = (int)sSize;
			for (int row = 0; row < size; row++)
			{
				for (int col = 0; col < size; col++)
				{
					target[row, col].Value = source[row, col];
				}
			}

		}
		public static void CopyCells(List<CellDTO> sources, Cell[,] target, SudokuSize sSize)
		{
			int size = (int)sSize;
			for (int row = 0; row < size; row++)
			{
				for (int col = 0; col < size; col++)
				{
					var source = sources.FirstOrDefault(c => c.Row == row && c.Column == col);
					target[row, col].Value = source.Value;
				}
			}

		}
		public static Cell[,] BuildCells(SudokuSize sSize)
		{
			int size = (int)sSize;
			var ret = new Cell[size, size];

			for (int row = 0; row < size; row++)
			{
				for (int col = 0; col < size; col++)
				{
					ret[row, col] = new Cell()
					{
						Coordinate = new Coordinate
						{
							Row = row,
							Column = col
						}
					};
				}
			}
			ret.ToList().ForEach(cell =>
			{
				cell.Column = GetColumn(ret, cell.Coordinate.Column);
				cell.Row = GetRow(ret, cell.Coordinate.Row);
				cell.Quadrant = GetQuadrant(ret, cell.Coordinate.Row, cell.Coordinate.Column);
			});
			return ret;
		}

		public static T[] GetColumn<T>(T[,] cells, int column)
		{
			return Enumerable.Range(0, cells.GetLength(0))
					.Select(x => cells[x, column])
					.ToArray();
		}
		public static T[] GetRow<T>(T[,] cells, int row)
		{
			return Enumerable.Range(0, cells.GetLength(1))
					.Select(x => cells[row, x])
					.ToArray();
		}
		public static T[,] GetQuadrant<T>(T[,] cells, int row, int column)
		{
			T[,] quad = new T[3, 3];
			var rowSet = QuadSet(row);
			var colSet = QuadSet(column);
			for (int r = rowSet[0]; r <= rowSet[2]; r++)
			{
				for (int c = colSet[0]; c <= colSet[2]; c++)
				{
					quad[r % 3, c % 3] = cells[r, c];
				}
			}
			return quad;
		}

		public static Quadrant GetQuadrant(int row, int column)
		{
			int quadRow = QuadSets.IndexOf(QuadSets.First(qs => qs.Contains(row)));
			int quadCol = QuadSets.IndexOf(QuadSets.First(qs => qs.Contains(column)));
			return QuadrantsByCoordinates[quadRow][quadCol];
		}
		public static Cell[,] GetQuadrant(Cell[,] cells, Quadrant quad)
		{
			switch (quad)
			{
				case Quadrant.UpperLeft:
					return GetQuadrant(cells, 0, 0);
				case Quadrant.UpperMiddle:
					return GetQuadrant(cells, 0, 3);
				case Quadrant.UpperRight:
					return GetQuadrant(cells, 0, 6);
				case Quadrant.MiddleLeft:
					return GetQuadrant(cells, 3, 0);
				case Quadrant.MiddleMiddle:
					return GetQuadrant(cells, 3, 3);
				case Quadrant.MiddleRight:
					return GetQuadrant(cells, 3, 6);
				case Quadrant.LowerLeft:
					return GetQuadrant(cells, 6, 0);
				case Quadrant.LowerMiddle:
					return GetQuadrant(cells, 6, 3);
				case Quadrant.LowerRight:
					return GetQuadrant(cells, 6, 6);
				default: return null;
			}
		}

		private static List<Quadrant[]> _quadrantsByCoordinates;
		private static List<Quadrant[]> QuadrantsByCoordinates
		{
			get
			{
				if (_quadrantsByCoordinates == null)
				{
					_quadrantsByCoordinates = new List<Quadrant[]>();
					List<Quadrant> allQuads = new List<Quadrant>();
					foreach (var quad in Enum.GetValues(typeof(Quadrant)))
					{
						allQuads.Add((Quadrant)quad);
					}
					int count = 0;
					for (int r = 0; r < 3; r++)
					{
						Quadrant[] q = new Quadrant[3];
						for (int c = 0; c < 3; c++)
						{
							q[c] = allQuads[count];
							count++;
						}
						_quadrantsByCoordinates.Add(q);
					}
				}
				return _quadrantsByCoordinates;
			}
		}
		private static List<int[]> QuadSets = new List<int[]>()
		{
			new int[] { 0, 1, 2 },
			new int[] { 3, 4, 5 },
			new int[] { 6, 7, 8 }
		};
		public static int[] QuadSet(int val)
		{
			return QuadSets.FirstOrDefault(qs => qs.Contains(val));
		}

	}
}
