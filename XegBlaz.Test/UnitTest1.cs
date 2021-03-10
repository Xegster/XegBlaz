using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using XegSweeper.Models;
using XegSweeper.Utilities;

namespace XegBlaz.Test
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			Board b = new Board(DifficultySettings.Easy);
		}
	}
}
