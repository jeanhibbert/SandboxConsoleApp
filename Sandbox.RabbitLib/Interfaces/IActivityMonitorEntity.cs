using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Interfaces
{
    public interface IActivityMonitorEntity
    {
        Guid ActivityId { get; set; }
    }
}
