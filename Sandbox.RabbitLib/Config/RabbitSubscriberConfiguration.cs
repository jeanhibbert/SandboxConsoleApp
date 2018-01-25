using Sandbox.Core;
using Sandbox.RabbitLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Config
{
    public class RabbitSubscriberConfiguration : IRabbitSubscriberConfiguration
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

        public string Queue
        {
            get
            {
                return (ConfigurationManager.AppSettings["Rabbit.CommandControl.DynamicConfig.Queue"] ?? "CommandControl.DynamicConfig")
                    .ReplaceTokens(new { Environment.MachineName });
            }
        }

        public string BindingKey
        {
            get
            {
                return (ConfigurationManager.AppSettings["Rabbit.CommandControl.DynamicConfig.BindingKey"] ?? "DynamicConfig.*")
                    .ReplaceTokens(new { Environment.MachineName });
            }
        }
    }
}
