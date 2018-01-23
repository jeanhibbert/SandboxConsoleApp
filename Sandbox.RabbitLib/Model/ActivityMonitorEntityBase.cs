using Sandbox.RabbitLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Model
{
    public abstract class ActivityMonitorEntityBase : IActivityMonitorEntity
    {
        protected ActivityMonitorEntityBase()
        {
            ActivityId = Guid.NewGuid();
        }

        public Guid ActivityId { get; set; }

        public string ExternalRef { get; set; }
    }
}
