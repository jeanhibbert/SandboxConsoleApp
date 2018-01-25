using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Model
{
    public class TestRabbitMessage
    {
        public IDictionary<string, object> Headers { get; set; }
        public byte[] Payload { get; set; }
    }
}
