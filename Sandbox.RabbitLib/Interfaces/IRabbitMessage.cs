using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib
{
    public interface IRabbitMessage
    {
        Guid MessageId { get; set; }
        IDictionary<string, object> Headers { get; }
        byte[] Payload { get; }
    }
}
