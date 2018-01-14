using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SandboxConsoleApp.Http
{
    public class SearchController : ApiController
    {
        [HttpPost]
        //[MustBeMemberOf(Roles = SecurityRoles.Developer)]
        public HttpResponseMessage Search(SearchRequest request)
        {
            SearchResponse response = new SearchResponse
            {
                 Page = 1,
                 SearchResults = new List<SearchResult> { new SearchResult { Data = "Some Data" } }
            };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
