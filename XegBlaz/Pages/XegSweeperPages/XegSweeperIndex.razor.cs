using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;
using XegSweeper;
using XegSweeper.Models;
using XegSweeper.Utilities;

namespace XegBlaz.Pages.XegSweeperPages
{
	public partial class XegSweeperIndex
	{
		protected bool Debug { get; set; } = false;
		protected Board CurrentBoard { get; set; }
		protected Settings CurrentSettings { get; set; }
		protected Settings CustomSettings { get; set; }
		protected bool GameStarted { get; set; }
		protected GameState CurrentState { get { return CurrentBoard.CurrentState; } set { CurrentBoard.CurrentState = value; } }

		protected bool Lost { get { return CurrentState == GameState.Lost; } }
		protected bool Won { get { return CurrentState == GameState.Won; } }

		#region Clicks
		protected void SetSettings(Settings settings)
		{
			CurrentSettings = settings;
		}
		protected void SetCustom()
		{
			if (CustomSettings == null)
			{
				CustomSettings = new Settings
				{
					Height = 20,
					Width = 20,
					MineCount = 20
				};
			}
			CurrentSettings = CustomSettings;
		}
		protected void StartGame()
		{
			Console.WriteLine("Game Started");

			CurrentBoard = XegSweeperFactory.BuildBoard(CurrentSettings);
			GameStarted = true;
		}
		protected void Uncover(int row, int column)
		{
			if (CurrentState != GameState.InProgress) return;

			Console.WriteLine("Row: " + row + " Col: " + column);
			if (!TimeStarted)
				RunGameTimer();
			CurrentBoard.Uncover(row, column);//
			CheckGameState();
		}
		protected void SetFlag(MouseEventArgs args, int row, int column)
		{
			if (args.Button == 2)
			{
				CurrentBoard.Flag(row, column);
			}
		}
		protected void StartOver()
		{
			TimeRunning = false;
			TimeStarted = false;
			CurrentBoard = null;
			CurrentSettings = null;
			GameStarted = false;
		}

		#endregion
		private void CheckGameState()
		{
			if (CurrentBoard.CurrentState != XegSweeper.Utilities.GameState.InProgress)
			{
				TimeRunning = false;
			}
		}

		#region Timer
		public TimeSpan GameTime { get; set; }
		public long StartTime { get; set; }
		public bool TimeRunning { get; set; }
		public bool TimeStarted { get; set; }
		public async Task RunGameTimer()
		{
			GameTime = new TimeSpan();
			TimeStarted = true;
			TimeRunning = true;
			StartTime = DateTime.UtcNow.Ticks;
			while (TimeRunning)
			{
				await Task.Delay(100);
				if (TimeRunning)
				{
					//GameTime = GameTime.Add(new TimeSpan(0, 0, 0, 0, 1000));
					GameTime = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - StartTime);
					StateHasChanged();
				}
			}
		}
		#endregion
	}
}
