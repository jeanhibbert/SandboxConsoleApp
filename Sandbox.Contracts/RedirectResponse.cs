using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SandboxConsoleApp.Http
{
    public class RedirectResponse : ContentResponse
    {
        public string RedirectToLocation { get; set; }

        public string HttpStatusDescription()
        {
            var description = string.Format("{0} ({1}", (int)StatusCode, StatusCode);

            if (StatusCode == HttpStatusCode.Redirect)
            {
                description += string.Format(" -> {0}", RedirectToLocation);
            }

            description += ")";
            return description;
        }
    }
}
