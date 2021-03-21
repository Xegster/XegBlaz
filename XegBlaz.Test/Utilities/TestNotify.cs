using XegDoKu.Interfaces;
using XegDoKu.Models;

namespace XegBlaz.Test.Utilities
{
	public class TestNotify : ISolverNotify
	{
		public int BoardAddedCount { get; set; }
		public int BoardModifiedCount { get; set; }
		public void BoardAdded(Board board)
		{
			BoardAddedCount++;
		}
		public void BoardModified(Board board)
		{
			BoardModifiedCount++;
		}
	}
}
