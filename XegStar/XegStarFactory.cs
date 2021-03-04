using System.Collections.Generic;
using XegStar.Models;
using XegStar.Utilities;

namespace XegStar
{
	public class XegStarFactory
	{
		public XegStarFactory()
		{

		}
		public static XegStarGame BuildBasicGame()
		{
			var ret = new XegStarGame
			{
				CurrentState = GameState.ChoosePlayers,
				Settings = new Settings(),
				Players = new List<Player>()
				{
					new Player
					{
						Id = 1,
						Name = "Player 1"
					},
					new Player
					{
						Id = 2,
						Name = "Player 2"
					}
				},
				Slots = new List<Slot>()
				{
					new Slot
					{
						Multiplier = 3.0,
					},
					new Slot
					{
						Multiplier = 2.0
					},
					new Slot
					{
						Multiplier = 4.0
					},
					new Slot
					{
						Multiplier = 2.0
					},
					new Slot
					{
						Multiplier = 1.0
					},
					new Slot
					{
						Multiplier = 1.0
					},
				},
			};
			foreach (var player in ret.Players)
			{
				var tokens = Defaults.BuildBasicTokens();
				tokens.ForEach(token => token.Owner = player);
				player.Tokens = tokens;
			}
			return ret;
		}
	}
}
