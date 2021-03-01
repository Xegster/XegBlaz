using System;
using System.Collections.Generic;
using System.Linq;

namespace XegStar.Utilities
{
	public static class Extensions
	{
		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

	}
}
