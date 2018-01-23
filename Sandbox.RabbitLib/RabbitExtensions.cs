using RabbitMQ.Client;
using Sandbox.Core;
using Sandbox.RabbitLib.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib
{
    public static class RabbitExtensions
    {
        public static void SafeDispose(this IConnection connection)
        {
            if (connection == null)
            {
                return;
            }

            try
            {
                connection.Dispose();
            }
            catch (IOException)
            {
                // swallow this explicit error - Rabbit always throws this if closing an an already closed connection
            }
        }

        public static RabbitMessage PayloadIsJsonOf<T>(this RabbitMessage target, T payloadSource)
        {
            var payload = Encoding.UTF8.GetBytes(payloadSource.ToJson());
            target.PayloadIs(payload);
            return target;
        }
    }
}
