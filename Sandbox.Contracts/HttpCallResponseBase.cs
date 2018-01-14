using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SandboxConsoleApp.Http
{
    public abstract class HttpCallResponseBase
    {
        public bool Faulted { get; set; }

        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public HttpErrorContent HttpError { get; set; }
    }
}
