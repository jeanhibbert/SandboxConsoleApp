using Sandbox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Model
{
    public class RabbitConfig : IRabbitConfiguration
    {
        private string _queue;
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string BindingKey { get; set; }

        public string Queue
        {
            get { return _queue; }
            set { _queue = value.ReplaceTokens(new { Environment.MachineName }); }
        }
    }
}
