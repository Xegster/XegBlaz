using System.Collections.Generic;

namespace XegStar.Models
{
	public class Player
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Token> Tokens { get; set; }
	}
}
