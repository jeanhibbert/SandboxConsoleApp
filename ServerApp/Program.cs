using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServerApp.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.SelfHost;

namespace ServerApp
{
    class Program
    {
        private static HttpSelfHostServer _server;
        static void Main(string[] args)
        {
            //netsh http add urlacl url=http://+:8282/testapp/v1/ user=XYZ\ABC
            var selfhostConfig = new WindowsAuthSelfHostConfiguration("http://localhost:8282/testapp/v1")
            {
                //DependencyResolver = new UnityDependencyResolver(container),
                MaxReceivedMessageSize = int.MaxValue
            };

            Configure(selfhostConfig);
            
            _server = new HttpSelfHostServer(selfhostConfig);
            _server.OpenAsync().Wait();

            Console.WriteLine("Server started...");
            Console.ReadKey();
        }



        private static void Configure(HttpSelfHostConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
          
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            var jsonFormatter = config.Formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute(
                "ActionSpecific",
                "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            //config.Filters.Add(new ActionLoggingFilterAttribute());
            //config.Filters.Add(new LoggingExceptionFilterAttribute());

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }

        private static object GetServerSelfHostConfiguration(object appConfig, object container)
        {
            throw new NotImplementedException();
        }
    }
}
