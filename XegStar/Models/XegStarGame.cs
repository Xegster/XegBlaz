using System.Collections.Generic;
using System.Linq;
using XegStar.Utilities;

namespace XegStar.Models
{
	public class XegStarGame
	{
		public List<Slot> Slots { get; set; }
		public List<Token> Tokens { get; set; }
		public List<Player> Players { get; set; }
		public Settings Settings { get; set; }

		public GameState CurrentState { get; set; }

		public WinType WinType { get; set; }
		public void TakeTurn(Slot targetSlot, Token dragToken)
		{
			targetSlot.AddToken(dragToken);
			if (!GameFinished())
			{
				if (CurrentState == GameState.Player1Turn)
					CurrentState = GameState.Player2Turn;
				else
					CurrentState = GameState.Player1Turn;
			}
		}

		public Player WinningPlayer()
		{
			return Slots.Where(slot => slot.CurrentToken != null)
						.Select(slot => slot.CurrentToken.Owner)
						.DistinctBy(player => player.Id)
						.OrderByDescending(player => Score(player))
						.First();
		}



		private bool GameFinished()
		{
			var gameInProgress = Players.Any(player => player.Tokens.Any());
			if (gameInProgress) return false;
			else
			{
				CurrentState = GameState.Finish;

				return true;
			}
		}

		public int Score(Player player)
		{
			return Slots.Where(slot => slot.CurrentToken != null && slot.CurrentToken.Owner.Id == player.Id).Select(slot => slot.CurrentToken.Value).Sum();
		}
	}
}
