using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Interfaces
{
    public interface IRabbitSubscriberConfiguration : IRabbitConfiguration
    {
        string Queue { get; }

        // required if you want to create the subscription queue/binding
        string Exchange { get; }
        string BindingKey { get; }
    }
}
