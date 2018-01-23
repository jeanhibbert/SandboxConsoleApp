using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Model
{
    public class RabbitMessage : IRabbitMessage
    {
        private readonly Dictionary<string, object> _headers;

        public RabbitMessage()
        {
            _headers = new Dictionary<string, object>();
        }

        public Guid MessageId { get; set; }

        public IDictionary<string, object> Headers { get { return _headers; } }

        public byte[] Payload { get; private set; }

        public static RabbitMessage WithMessageId(Guid id)
        {
            return new RabbitMessage { MessageId = id };
        }

        public RabbitMessage SetHeader(string name, object value)
        {
            _headers.Add(name, value);
            return this;
        }

        public RabbitMessage PayloadIs(byte[] payload)
        {
            Payload = payload;
            return this;
        }
    }
}
