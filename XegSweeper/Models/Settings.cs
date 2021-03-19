namespace XegSweeper.Models
{
	public class Settings
	{
		public int MaxWidth { get; } = 100;
		public int MaxHeight { get; } = 100;
		public int MinWidth { get; } = 1;
		public int MinHeight { get; } = 1;
		public int MinMineCount { get; } = 1;
		public int MaxMineCount { get { return (Width * Height) - FreeHits - (FreeHits == 0 ? 1 : 0); } }
		public int MinFreeHits { get; } = 0;
		public int MaxFreeHits { get { return (Width * Height) - MineCount; } }

		public int Width { get; set; }
		public int Height { get; set; }
		public int MineCount { get; set; }
		public int FreeHits { get; set; }

	}
}
