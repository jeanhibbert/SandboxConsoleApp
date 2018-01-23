using RabbitMQ.Client;
using Sandbox.Core;
using Sandbox.RabbitLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Services
{
    public class SandboxConsumer : DefaultBasicConsumer
    {
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            if (routingKey.EndsWith("DynamicConfig.SetValue"))
            {
                var args = Encoding.UTF8.GetString(body).FromJson<CommandArgs>();
                args.ExternalRef = deliveryTag.ToString();
                return;
            }

            //// add more commands here...

            //Log.Warning(
            //    "Unsupported routing key {RabbitMessageRoutingKey} for {RabbitMessageConsumerType}",
            //    routingKey,
            //    GetType().Name);
        }
    }
}
