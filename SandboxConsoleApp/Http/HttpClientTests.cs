using System;
using System.Linq;
using System.Threading.Tasks;

namespace SandboxConsoleApp.Http
{
    public class HttpClientTests
    {
        public static void TestHttpConnector()
        {
            var response = Search(new SearchRequest { Data = "123" });
            Console.WriteLine($"Got a response!!! :{response.SearchResults.First().Data}");
        }

        public static void TestAsyncHttpConnector()
        {
            var task = SearchAsync(new SearchRequest { Data = "123" }, "http://localhost:8282/testapp/v1/api/search/search");
            Console.WriteLine("Completed run...");
            Console.WriteLine($"Got a response!!! :{task.Result.SearchResults.First().Data}") ;
        }

        private static async Task<SearchResponse> SearchAsync(SearchRequest searchRequest, string url)
        {
            using (var client = new HttpConnector())
            {
                var response = await client.PostAsJsonAsync<SearchRequest, SearchResponse>(searchRequest, url);
                return response;
            }
        }

        public static SearchResponse Search(SearchRequest request)
        {
            using (var client = new HttpConnector())
            {
                var url = new TestHttpClientConfig().BaseUrl.ToUrl("api/search/search");
                var apiResponse = client.PostAsJson<SearchRequest, SearchResponse>(request, url);
                return apiResponse.Data;
            }
        }

    }
}
