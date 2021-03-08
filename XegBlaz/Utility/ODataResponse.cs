using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XegBlaz
{
	public class ODataResponse<T>
	{
		[JsonPropertyName("@odata.count")]
		public int Count { get; set; }
		public List<T> Value { get; set; }
	}
}
