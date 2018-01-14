using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxConsoleApp.Http
{
    public class SearchResponse
    {
        public int Page { get; set; }
        public IEnumerable<SearchResult> SearchResults { get; set; }
    }
}
