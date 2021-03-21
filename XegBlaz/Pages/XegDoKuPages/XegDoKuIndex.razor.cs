using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XegDoKu.Models;
using XegDoKu.Utilities;

namespace XegBlaz.Pages.XegDoKuPages
{
	public partial class XegDoKuIndex
	{
		public XegDoKuGame CurrentGame { get; set; }

		public List<Board> Boards { get; set; } = new List<Board>();

		public int BoardWidth { get; set; } = 3;

		protected override async Task OnInitializedAsync()
		{
			var settings = new Settings()
			{
				Size = XegDoKu.Utilities.SudokuSize.Standard,
				Difficulty = XegDoKu.Utilities.Difficulty.Easy
			};

			var rawJson = await Http.GetStringAsync("sample-data/TestSolution.json");
			List<CellDTO> solution = JsonConvert.DeserializeObject<List<CellDTO>>(rawJson);

			rawJson = await Http.GetStringAsync("sample-data/TestProblem.json");
			List<CellDTO> problem = JsonConvert.DeserializeObject<List<CellDTO>>(rawJson);

			//CurrentGame = XegDoKuFactory.BuildBasicGame();
			//var problem = await Http.GetJsonAsync<List<CellDTO>>("sample-data/TestProblem.json");

			//var solution = await Http.GetJsonAsync<List<CellDTO>>("sample-data/TestSolution.json");
			Console.WriteLine(string.Join(",", problem.Select(p => p.Value).ToArray()));
			Console.WriteLine(string.Join(",", solution.Select(p => p.Value).ToArray()));
			CurrentGame = new XegDoKuGame
			{
				Board = new Board(settings, solution)
			};

			Boards.Add(new Board(settings, problem));
			Boards.Add(new Board(settings, solution));
		}
		protected string CellClass(int row, int column)
		{
			List<string> classes = new List<string>();
			switch (column)
			{
				case 2:
				case 5:
					classes.Add("thick-right");
					break;
				case 3:
				case 6:
					classes.Add("thick-left");
					break;
			}
			switch (row)
			{
				case 2:
				case 5:
					classes.Add("thick-bottom");
					break;
				case 3:
				case 6:
					classes.Add("thick-top");
					break;
			}
			Quadrant q = Utilities.GetQuadrant(row, column);
			if (((int)q % 2) == 1)
				classes.Add("quadrant-odd");
			else
				classes.Add("quadrant-even");
			return string.Join(" ", classes);

		}
	}
}
