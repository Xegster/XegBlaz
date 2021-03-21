using XegDoKu.Utilities;

namespace XegDoKu.Models
{
	public class Settings
	{
		public SudokuSize Size { get; set; }
		public Difficulty Difficulty { get; set; }
		public static Settings EasyStandard
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
		public static Settings MediumStandard
		{
			get
			{
				return new Settings
				{
					Size = SudokuSize.Standard,
					Difficulty = Difficulty.Medium
				};
			}
		}
		public static Settings HardStandard
		{
			get
			{
				return new Settings
				{
					Size = SudokuSize.Standard,
					Difficulty = Difficulty.Hard
				};
			}
		}
		public static Settings ImpossibleStandard
		{
			get
			{
				return new Settings
				{
					Size = SudokuSize.Standard,
					Difficulty = Difficulty.Impossible
				};
			}
		}
		public static Range ClueCount(Difficulty difficulty)
		{
			switch (difficulty)
			{
				case Difficulty.Impossible:
					return new Range { Minimum = 17, Maximum = 30 };
				case Difficulty.Hard:
					return new Range { Minimum = 31, Maximum = 35 };
				case Difficulty.Medium:
					return new Range { Minimum = 36, Maximum = 40 };
				case Difficulty.Easy:
					return new Range { Minimum = 41, Maximum = 45 };
				default:
					return ClueCount(Difficulty.Easy);
			}
		}
	}
}
