using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib
{
    public interface IRabbitConfiguration
    {
        string HostName { get; }
        string VirtualHost { get; }
        int Port { get; }
        string UserName { get; }
        string Password { get; }
    }
}
