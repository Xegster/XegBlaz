using XegStar;
using XegStar.Models;
using XegStar.Utilities;
namespace XegBlaz.Pages.XegStarPages
{
	public partial class XegStarIndex
	{
		#region Game Properties
		protected XegStarGame CurrentGame { get; set; }

		protected Player Player1 => CurrentGame.Players[0];
		protected Player Player2 => CurrentGame.Players[1];
		protected int Player1Score => CurrentGame.Score(Player1);
		protected int Player2Score => CurrentGame.Score(Player2);

		protected Token DragToken { get; set; }
		protected Slot TargetSlot { get; set; }
		#endregion

		#region Front End Properties
		protected GameState CurrentState
		{
			get { return CurrentGame.CurrentState; }
			set { CurrentGame.CurrentState = value; }
		}
		protected WinType WinType { get { return CurrentGame.WinType; } }
		protected Player Winner { get { return CurrentGame.WinningPlayer(); } }
		#endregion

		#region Clicks
		protected void SetPlayerCount(int count)
		{
			CurrentGame.Settings.NumberOfPlayers = count;
			if (count == 1)
				CurrentState = GameState.ChooseDifficulty;
			else
				CurrentState = GameState.Player1Turn;
		}
		protected void SetDifficulty(Difficulty difficulty)
		{
			CurrentGame.Settings.Difficulty = difficulty;
			CurrentState = GameState.Player1Turn;
		}
		#endregion

		#region CSS Class
		public string PlayerClass(int player)
		{
			var player1Turn = player == 1 && CurrentState == GameState.Player1Turn;
			var player2Turn = player == 2 && CurrentState == GameState.Player2Turn;
			if (player1Turn || player2Turn)
			{
				return "player-turn";
			}
			else
			{
				return "player-wait";
			}

		}
		public string Draggable(int player)
		{
			var player1Turn = player == 1 && CurrentState == GameState.Player1Turn;
			var player2Turn = player == 2 && CurrentState == GameState.Player2Turn;
			if (player1Turn || player2Turn)
			{
				return "true";
			}
			else
			{
				return "false";
			}

		}
		#endregion
		protected override void OnInitialized()
		{
			CurrentGame = XegStarFactory.BuildBasicGame();
		}
		private void HandleDragStart(Token selectedToken)
		{
			DragToken = selectedToken;
		}
		protected void HandleDragEnter(Slot slot)
		{
			TargetSlot = slot;

		}
		protected void HandleDragLeave()
		{
			//TargetSlot = null;
		}
		private void HandleDrop()
		{
			CurrentGame.TakeTurn(TargetSlot, DragToken);

		}

	}
}
