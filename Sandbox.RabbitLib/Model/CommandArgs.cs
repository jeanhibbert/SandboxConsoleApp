using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Model
{
    public class CommandArgs : ActivityMonitorEntityBase
    {
        public string RequestedBy { get; set; }
        public string Target { get; set; }
        public string Value { get; set; }
    }
}
