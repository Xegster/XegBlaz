using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using XegDoKu;
using XegDoKu.Models;
using XegDoKu.Utilities;
using Utils = XegDoKu.Utilities.Utilities;
namespace XegBlaz.Test
{
	[TestClass]
	public class XegDoKuTest
	{
		private int[,] TestSudoku = new int[,]
			{
				{ 9,0,0,0,0,6,7,0,2 },
				{ 0,0,7,0,0,0,0,6,0 },
				{ 0,1,6,2,0,0,0,0,0 },
				{ 0,0,0,0,2,9,3,5,0 },
				{ 4,0,0,7,0,8,0,0,6 },
				{ 0,2,5,1,4,0,0,0,0 },
				{ 0,0,0,0,0,4,8,2,0 },
				{ 0,6,0,0,0,0,5,0,0 },
				{ 5,0,9,8,0,0,0,0,1 },
			};
		private Board TestBoard;
		[TestInitialize]
		public void Setup()
		{
			TestBoard = new Board(Settings.BasicSettings, TestSudoku);
			var jsonData = TestBoard.Cells.ToList().Select(cell => new CellDTO
			{
				Value = cell.Value,
				Row = cell.Coordinate.Row,
				Column = cell.Coordinate.Column
			}).ToList();
			var json = JsonConvert.SerializeObject(jsonData);
			AddToFile(@"C:\Xegster\Development\XegBlaz\XegBlaz\wwwroot\sample-data\TestProblem.json", json, false);

		}

		[TestMethod]
		public void TestEnumOrder()
		{
			List<string> ret = new List<string>();
			foreach (var quad in Enum.GetValues(typeof(Quadrant)))
				ret.Add(((Quadrant)quad).ToString());
			var x = string.Join(",", ret);
		}

		[TestMethod]
		public void TestSolver()
		{
			Console.WriteLine("This is a test output.");
			var solver = new XegDoKuSolver(TestBoard);
			var solution = solver.Solve();
			var jsonData = solution.SolvedBoard.Cells.ToList().Select(cell => new CellDTO
			{
				Value = cell.Value,
				Row = cell.Coordinate.Row,
				Column = cell.Coordinate.Column
			}).ToList();
			var json = JsonConvert.SerializeObject(jsonData);
			AddToFile(@"C:\Xegster\Development\XegBlaz\XegBlaz\wwwroot\sample-data\TestSolution.json", json, false);
		}

		[TestMethod]
		public void TestDeserialize()
		{
			var rawJson = File.ReadAllText(@"C:\Xegster\Development\XegBlaz\XegBlaz\wwwroot\sample-data\TestSolution.json");
			List<CellDTO> jsonData = JsonConvert.DeserializeObject<List<CellDTO>>(rawJson);
			var b = new Board(Settings.BasicSettings, jsonData);

		}
		public static void AddToFile(string file, string content, bool append = true)
		{
			try
			{
				if (!Directory.Exists(Path.GetDirectoryName(file)))
					Directory.CreateDirectory(Path.GetDirectoryName(file));
				using (StreamWriter sr = new StreamWriter(file, append))
				{
					sr.WriteLine(content);
				}
			}
			catch { }
		}


		[TestMethod]
		public void TestClone()
		{
			var TestCells = TestBoard.Cells;
			var clonedBoard = Utils.BuildCells(XegDoKu.Utilities.SudokuSize.Standard);
			TestCells[0, 0].ForbiddenValues.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
			Utils.CopyCells(TestCells, clonedBoard, XegDoKu.Utilities.SudokuSize.Standard);
			TestCells[0, 0].Value += 200;
			Assert.IsFalse(TestCells[0, 0].Value == clonedBoard[0, 0].Value);
			Assert.IsTrue(Enumerable.SequenceEqual(TestCells[0, 0].ForbiddenValues.OrderBy(ob => ob), clonedBoard[0, 0].ForbiddenValues.OrderBy(ob => ob)));

			TestCells[0, 0].ForbiddenValues.Add(69);

			Assert.IsFalse(Enumerable.SequenceEqual(TestCells[0, 0].ForbiddenValues.OrderBy(ob => ob), clonedBoard[0, 0].ForbiddenValues.OrderBy(ob => ob)));
			TestCells[0, 0].ForbiddenValues.Remove(69);
			Assert.IsTrue(Enumerable.SequenceEqual(TestCells[0, 0].ForbiddenValues.OrderBy(ob => ob), clonedBoard[0, 0].ForbiddenValues.OrderBy(ob => ob)));
			TestCells[0, 0].ForbiddenValues = new List<int>() { 420, 69 };
			Assert.IsFalse(Enumerable.SequenceEqual(TestCells[0, 0].ForbiddenValues.OrderBy(ob => ob), clonedBoard[0, 0].ForbiddenValues.OrderBy(ob => ob)));

		}


		[TestMethod]
		public void TestTime()
		{
			Stopwatch st = new Stopwatch();
			st.Start();
			for (int i = 0; i < 1000; i++)
			{
				var xegdoku = XegDoKu.XegDoKuFactory.BuildBasicGame();

			}
			st.Stop();
			var result = TimeSpan.FromTicks(st.ElapsedTicks);
		}

		[TestMethod]
		public void TestMethods()
		{
			for (int i = 0; i < 1000; i++)
			{
				var xegdoku = XegDoKu.XegDoKuFactory.BuildBasicGame();
				var errors = XegDoKuChecker.VerifyBoard(xegdoku.Board);
				Assert.IsTrue(errors.Count == 0);

			}
		}

	}
}
