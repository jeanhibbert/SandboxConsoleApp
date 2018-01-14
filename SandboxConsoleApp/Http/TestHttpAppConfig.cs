using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxConsoleApp.Http
{
    public class TestHttpClientConfig
    {
        public string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["AppServerBaseUrl"]; }
        }
    }
}
