using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxConsoleApp.Http
{
    public class HttpClientTests
    {
        public static void TestHttpClient()
        {
            var response = Search(new SearchRequest { Data = "123" } );
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
