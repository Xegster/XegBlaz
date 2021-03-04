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
		/// <summary>
		/// Stolen from http://tech.pro/blog/2226/extension-method-distinct-objects
		/// </summary>
		/// <typeparam name="T">Any object.</typeparam>
		/// <typeparam name="TKey">A value to be used as the primary key on the object T</typeparam>
		/// <param name="source">An IEnumerable of objects T</param>
		/// <param name="keySelector">A function that returns the TKey value on an object T</param>
		/// <returns>Only the unique elements in the list.</returns>
		public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (T element in source)
				if (seenKeys.Add(keySelector(element)))
					yield return element;
		}
	}
}
