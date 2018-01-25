using Sandbox.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Model
{
    class RabbitPublisherConfiguration : IRabbitPublisherConfiguration
    {
        public string HostName { get { return ConfigurationManager.AppSettings["Rabbit.Host"] ?? "localhost"; } }
        public string VirtualHost { get { return ConfigurationManager.AppSettings["Rabbit.VirtualHost"] ?? "/"; } }

        public int Port
        {
            get
            {
                int port;
                return int.TryParse(ConfigurationManager.AppSettings["Rabbit.Port"], out port)
                    ? port
                    : 5673;
            }
        }

        public string UserName { get { return ConfigurationManager.AppSettings["Rabbit.Username"] ?? "guest"; } }
        public string Password { get { return ConfigurationManager.AppSettings["Rabbit.Password"] ?? "guest"; } }

        public string Exchange
        {
            get
            {
                return (ConfigurationManager.AppSettings["Rabbit.CommandControl.Exchange"] ?? "CommandControl")
                    .ReplaceTokens(new { Environment.MachineName });
            }
        }

        public string RoutingKey
        {
            get
            {
                return (ConfigurationManager.AppSettings["Rabbit.CommandControl.DynamicConfig.SetValue.RoutingKey"] ?? "DynamicConfig.SetValue")
                    .ReplaceTokens(new { Environment.MachineName });
            }
        }
    }
}
