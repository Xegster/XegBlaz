using Microsoft.VisualStudio.TestTools.UnitTesting;
using XegSweeper.Models;
using XegSweeper.Utilities;

namespace XegBlaz.Test
{
	[TestClass]
	public class XegSweeperTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			Board b = new Board(DifficultySettings.Easy);
		}
	}
}
