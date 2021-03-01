using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XegBlaz.Pages
{
	public partial class Counter
	{
		private int currentCount = 0;
		protected Dictionary<string, object> MyCodeGeneratedAttributes { get; set; }

		private void IncrementCount()
		{
			currentCount++;
		}
		protected override void OnInitialized()
		{
			MyCodeGeneratedAttributes = new Dictionary<string, object>();
			for (int index = 1; index <= 5; index++)
			{
				MyCodeGeneratedAttributes["attribute_" + index] = index;
			}
		}

	}
}
