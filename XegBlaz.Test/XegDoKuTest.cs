using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XegBlaz.Test.Utilities;
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
{ 8,0,6,0,5,0,0,2,9 },
{ 9,0,7,2,0,6,8,1,5 },
{ 1,5,2,9,8,0,0,6,3 },
{ 3,8,0,5,6,2,9,7,0 },
{ 5,2,0,8,7,9,6,3,0 },
{ 6,7,9,0,0,0,2,5,8 },
{ 4,9,3,7,2,5,1,8,6 },
{ 2,1,8,6,9,3,5,4,7 },
{ 7,6,5,0,0,8,3,9,2 },
			};
		private Board TestBoard;
		private string SolutionJsonName = "TestSolution.json";
		private string ProblemJsonName = "TestProblem.json";
		[TestInitialize]
		public void Setup()
		{
			TestBoard = new Board(Settings.EasyStandard, TestSudoku);
		}

		[TestMethod]
		public void TestAsync()
		{
			List<Task<List<Solution>>> tasks = new List<Task<List<Solution>>>();
			var st = Stopwatch.StartNew();
			TestNotify tn = new TestNotify();
			for (int i = 0; i < 100; i++)
			{
				var rando = XegDoKuFactory.BuildBase(Settings.EasyStandard);
				var randoBoard = new Board(Settings.EasyStandard, rando);
				//SaveToJSON(randoBoard, ProblemJsonName);
				var solver = new XegDoKuSolver(randoBoard, tn);
				tasks.Add(solver.SolveAsync());
			}

			TimeSpan creationTime = TimeSpan.FromTicks(st.ElapsedTicks);
			while (tasks.Any(t => !t.IsCompleted))
			{
				var task = tasks.FirstOrDefault(t => !t.IsCompleted);
				if (task != null)
					task.Wait();
			}
			st.Stop();
			TimeSpan totalTime = TimeSpan.FromTicks(st.ElapsedTicks);
			var uniqueSolutions = tasks.Where(t => t.Result.Count == 1).SelectMany(t => t.Result).ToList();
			var uniqueCount = uniqueSolutions.Count;
			var solutions = tasks.SelectMany(t => t.Result).ToList();
			
			var solvedSols = solutions.Where(s => s.State == SolutionState.Solved).ToList();
			var solvedCount = solvedSols.Count;
			var impossSols = solutions.Where(s => s.State != SolutionState.Solved).ToList();
			var impossCount = impossSols.Count;
			var boardCount = tn.BoardAddedCount;
			var boardMod = tn.BoardModifiedCount;

		}

		[TestMethod]
		public void TestFullGeneration()
		{
			List<Solution> solutions = new List<Solution>();
			var st = Stopwatch.StartNew();
			TestNotify tn = new TestNotify();
			for (int i = 0; i < 100; i++)
			{
				var rando = XegDoKuFactory.BuildBase(Settings.EasyStandard);
				var randoBoard = new Board(Settings.ImpossibleStandard, rando);
				//SaveToJSON(randoBoard, ProblemJsonName);
				var solver = new XegDoKuSolver(randoBoard, tn);
				var solution = solver.Solve();
				if (!solutions.Any(s => s.SolvedBoard.CompareCellValues(randoBoard).Count == 0))
					solutions.Add(solution);
				if (solution.State != SolutionState.Solved)
				{
					//SaveToJSON(solution.SolvedBoard, SolutionJsonName);
					//break;
				}
				//Assert.IsTrue(solution.State == SolutionState.Solved);
			}
			st.Stop();
			TimeSpan time = TimeSpan.FromTicks(st.ElapsedTicks);
			var solvedSols = solutions.Where(s => s.State == SolutionState.Solved).ToList();
			var solvedCount = solvedSols.Count;
			var impossSols = solutions.Where(s => s.State != SolutionState.Solved).ToList();
			var impossCount = impossSols.Count;
			var boardCount = tn.BoardAddedCount;
			var boardMod = tn.BoardModifiedCount;
			
		}

		[TestMethod]
		public void TestRandomness()
		{
			List<int[,]> randos = new List<int[,]>();
			var st = Stopwatch.StartNew();
			for (int i = 0; i < 100; i++)
			{
				var rando = XegDoKuFactory.BuildBase(Settings.EasyStandard);
				if (!randos.Any(r => r.Compare(rando).Count == 0))
					randos.Add(rando);
				
			}
			st.Stop();
			TimeSpan time = TimeSpan.FromTicks(st.ElapsedTicks);
			int count = randos.Count;
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
			var b = new Board(Settings.EasyStandard, jsonData);

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

		#region Helpers
		public static void SaveToJSON(Board board, string jsonName)
		{
			var jsonData = board.Cells.ToList().Select(cell => new CellDTO
			{
				Value = cell.Value,
				Row = cell.Coordinate.Row,
				Column = cell.Coordinate.Column
			}).ToList();
			var json = JsonConvert.SerializeObject(jsonData);
			AddToFile(@"C:\Xegster\Development\XegBlaz\XegBlaz\wwwroot\sample-data\" + jsonName, json, false);

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

		#endregion

	}
}
