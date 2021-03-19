using XegDoKu.Models;

namespace XegDoKu
{
	public class XegDoKuFactory
	{

		public static XegDoKuGame BuildBasicGame()
		{
			return new XegDoKuGame()
			{
				Board = new Board(new Settings() { Difficulty = Utilities.Difficulty.Easy, Size = Utilities.SudokuSize.Standard })
			};
		}
	}
}
