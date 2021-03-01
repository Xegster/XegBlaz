using System.Collections.Generic;

namespace XegStar.Models
{
	public class XegStarGame
	{
		public List<Slot> Slots { get; set; }
		public List<Token> Tokens { get; set; }
		public List<Player> Players { get; set; }
	}
}
