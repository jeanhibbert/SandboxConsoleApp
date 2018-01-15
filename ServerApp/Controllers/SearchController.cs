using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SandboxConsoleApp.Http
{
    public class SearchController : ApiController
    {
        [HttpPost]
        //[CustomAuth(Roles = SecurityRoles.Developer)]
        public HttpResponseMessage Search(SearchRequest request)
        {
            Console.WriteLine($"Recieved a request!! : {request.Data}");

            SearchResponse response = new SearchResponse
            {
                 Page = 1,
                 SearchResults = new List<SearchResult> { new SearchResult { Data = "Some Data" } }
            };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
