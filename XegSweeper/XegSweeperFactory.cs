using XegSweeper.Models;
using XegSweeper.Utilities;

namespace XegSweeper
{
	public class XegSweeperFactory
	{
		public static Board BuildEasyBoard()
		{
			return new Board(DifficultySettings.Easy);
		}
		public static Board BuildBoard(Settings settings)
		{
			return new Board(settings);
		}
	}
}
