namespace XegSweeper.Models
{
	public class Cell
	{
		public bool Bomb { get; set; }
		public int Value { get; set; }
		public bool IsEmpty { get { return Value == 0 && !Bomb; } }
		public bool Uncovered { get; set; } = false;
		public bool Covered { get { return !Uncovered; } set { Uncovered = !value; } }
		public bool Flagged { get; set; }
	}
}
