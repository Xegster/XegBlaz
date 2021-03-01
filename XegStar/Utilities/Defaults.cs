using System.Collections.Generic;
using XegStar.Models;

namespace XegStar.Utilities
{
	public class Defaults
	{
		public static List<Token> BuildBasicTokens()
		{
			var _basicTokens = new List<Token>();
			for (int i = 1; i < 5; i++)
				_basicTokens.Add(new Token { Value = i });
			return _basicTokens;
		}
	}
}
