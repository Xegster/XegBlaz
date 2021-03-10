namespace XegSweeper.Models
{
	public class Settings
	{
		public int Width { get; set; }
		public int Height { get; set; }
		public int MineCount { get; set; }
		public int FreeHits { get; set; }
		public int MaxMineCount { get { return (Width * Height) - FreeHits - (FreeHits == 0 ? 1 : 0); } }
		public int MaxFreeHits { get { return (Width * Height) - MineCount; } }
	}
}
