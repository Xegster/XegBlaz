using XegSweeper.Models;

namespace XegSweeper.Utilities
{
	public class DifficultySettings
	{
		private static Settings _easy;
		public static Settings Easy
		{
			get
			{
				if (_easy == null)
				{
					_easy = new Settings()
					{
						Height = 10,
						Width = 10,
						MineCount = 10
					};
				}
				return _easy;
			}
		}
		private static Settings _medium;
		public static Settings Medium
		{
			get
			{
				if (_medium == null)
				{
					_medium = new Settings()
					{
						Height = 14,
						Width = 18,
						MineCount = 40
					};
				}
				return _medium;
			}
		}
		private static Settings _hard;
		public static Settings Hard
		{
			get
			{
				if (_hard == null)
				{
					_hard = new Settings()
					{
						Height = 20,
						Width = 24,
						MineCount = 99
					};
				}
				return _hard;
			}
		}
	}
}
