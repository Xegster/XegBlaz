using XegDoKu.Models;

namespace XegDoKu.Interfaces
{
	public interface ISolverNotify
	{
		public void BoardAdded(Board board);
		public void BoardModified(Board board);
	}
}
