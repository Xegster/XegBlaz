using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XegDoKu.Utilities
{
	public class Range
	{
		public int Minimum { get; set; }
		public int Maximum { get; set; }
		private static Random Generator { get; set; } = new Random();
		public int GetRandom()
		{
			return Generator.Next(Minimum, Maximum);
		}
	}
}
