using System.Net.Http;

namespace XegBlaz.Services
{
	public class ApiService : IApiService
	{
		private HttpClient _httpClient;

		public ApiService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		//public async Task<ParserRun> ParserRunGetById(long parserRunId)
		//{
		//	//return await _httpClient.GetFromJsonAsync<ParserRun>($"v1/ParserRun({parserRunId})");
		//	var parserRuns = await _httpClient.GetFromJsonAsync<ODataResponse<ParserRun>>($"sample-data/parserrun.json");
		//	return parserRuns.Value.Where(x => x.Id == parserRunId).Single();
		//}

		//public async Task<ODataResponse<ParserRun>> ParserRun(int skip, int top, string orderBy)
		//{
		//	//return await _httpClient.GetFromJsonAsync<ODataResponse<ParserRun>>($"v1/ParserRun?$count=true&$skip={skip}&$top={top}&$orderby={orderBy}");
		//	return await _httpClient.GetFromJsonAsync<ODataResponse<ParserRun>>($"sample-data/parserrun.json");
		//}

		//public async Task<ODataResponse<ParserRunConfig>> ParserRunConfig(long parserRunId)
		//{
		//	//return await _httpClient.GetFromJsonAsync<ODataResponse<ParserRunConfigDTO>>($"v1/ParserRun({parserRunId})/ParserRunConfigs");
		//	return await _httpClient.GetFromJsonAsync<ODataResponse<ParserRunConfig>>($"sample-data/parserrunconfig.json");
		//}
	}
}
