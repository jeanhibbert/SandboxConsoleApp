using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Services
{
    using RabbitMQ.Client;
    using Sandbox.RabbitLib.Model;
    using System.Threading;

    public class RabbitService
    {

        public RabbitService()
        {
            UseRandomQueue();
        }

        public RabbitConfig Configuration { get; private set; }
        public string QueueName { get; private set; }
        public string Exchange { get; private set; }

        public RabbitService UseConfiguration(string configurationName)
        {
            Configuration = new RabbitConfig();
            return UseQueue(Configuration.Queue).UseExchange(Configuration.Exchange);
        }

        public RabbitService UseQueue(string queue)
        {
            QueueName = queue;
            return this;
        }

        public RabbitService UseExchange(string exchangeName)
        {
            Exchange = exchangeName;
            return this;
        }

        public RabbitService UseRandomQueue()
        {
            return UseQueue(Guid.NewGuid().ToString());
        }

        public RabbitService CreateQueue()
        {
            var factory = CreateConnectionFactory();

            using (var connection = factory.CreateConnection())
            {
                using (var model = connection.CreateModel())
                {
                    model.QueueDeclare(QueueName, false, false, false, null);
                    model.QueueBind(QueueName, Configuration.Exchange, Configuration.BindingKey);
                }
            }

            return this;
        }

        public RabbitService DeleteQueue()
        {
            var factory = CreateConnectionFactory();

            using (var connection = factory.CreateConnection())
            {
                using (var model = connection.CreateModel())
                {
                    model.QueueDelete(QueueName);
                }
            }

            return this;
        }

        public bool FindMessage(Guid messageId, out TestRabbitMessage message, int timeoutMilliseconds = 10000)
        {
            var factory = CreateConnectionFactory();

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var waitGate = new ManualResetEvent(false);

                    var consumer = new SpecificMessageRabbitConsumer(channel, waitGate, messageId);
                    channel.BasicConsume(QueueName, false, consumer);

                    // block here until we either find the message or timeout looking for it...
                    waitGate.WaitOne(timeoutMilliseconds);
                    message = consumer.Message;
                }
            }

            return message != null;
        }

        public void PublishMessage(string xml)
        {
            var factory = CreateConnectionFactory();

            var rabbitMessageToPublish = Encoding.Default.GetBytes(xml);

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.BasicPublish(Exchange, "#", null, rabbitMessageToPublish);
                }

                connection.Close();
            }
        }

        private ConnectionFactory CreateConnectionFactory()
        {
            var factory = new ConnectionFactory
            {
                HostName = Configuration.HostName,
                VirtualHost = Configuration.VirtualHost,
                Port = Configuration.Port,
                UserName = Configuration.UserName,
                Password = Configuration.Password
            };
            return factory;
        }

        public class SpecificMessageRabbitConsumer : DefaultBasicConsumer
        {
            private readonly IModel _channel;
            private readonly ManualResetEvent _waitGate;
            private readonly Guid _messageId;

            public SpecificMessageRabbitConsumer(IModel channel, ManualResetEvent waitGate, Guid messageId)
            {
                _channel = channel;
                _waitGate = waitGate;
                _messageId = messageId;
            }

            public TestRabbitMessage Message { get; private set; }

            public override void HandleBasicDeliver(
                string consumerTag,
                ulong deliveryTag,
                bool redelivered,
                string exchange,
                string routingKey,
                IBasicProperties properties,
                byte[] body)
            {
                var activityId = Encoding.Default.GetString((byte[])properties.Headers["ActivityId"]);
                if (activityId != string.Empty)
                {
                    if (_messageId.Equals(new Guid(activityId)))
                    {
                        Message = new TestRabbitMessage
                        {
                            Headers = properties.Headers,
                            Payload = body
                        };

                        _channel.BasicAck(deliveryTag, false);
                        _waitGate.Set();
                        return;
                    }
                }

                _channel.BasicReject(deliveryTag, true);
            }
        }
    }
}
