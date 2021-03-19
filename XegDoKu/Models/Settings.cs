using XegDoKu.Utilities;

namespace XegDoKu.Models
{
	public class Settings
	{
		public SudokuSize Size { get; set; }
		public Difficulty Difficulty { get; set; }


		public static Settings BasicSettings
		{
			get
			{
				return new Settings
				{
					Size = SudokuSize.Standard,
					Difficulty = Difficulty.Easy
				};
			}
		}
	}
}
