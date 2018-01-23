using RabbitMQ.Client;
using Sandbox.Core;
using Sandbox.RabbitLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.RabbitLib.Services
{
    public class RabbitSubscriber<TConsumer> where TConsumer : IBasicConsumer
    {

        private readonly TConsumer _consumer;

        private readonly IRabbitSubscriberConfiguration _subscriberConfig;

        private string _mqConsumerTag;

        public RabbitSubscriber(TConsumer consumer, IRabbitSubscriberConfiguration subscriberConfig)
        {
            _consumer = consumer;
            _subscriberConfig = subscriberConfig;

            Name = GetType().Name;
        }

        public string Name { get; private set; }

        protected IConnection MqConnection { get; set; }

        protected IModel MqChannel { get; set; }

        public virtual void Start()
        {
            CreateConnectionAndSubscriberChannel();
            CreateSubscription();
        }

        public virtual void Stop()
        {
            if (MqChannel == null)
            {
                return;
            }

            MqChannel.Dispose();
            MqConnection.SafeDispose();
        }

        private void CreateConnectionAndSubscriberChannel()
        {
            var factory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = false,
                RequestedHeartbeat = 30   // seconds
            };
            factory.InjectFrom(_subscriberConfig);

            MqConnection = factory.CreateConnection();
            //MqConnection.ConnectionShutdown += HandleMqConnectionShutdown;
            MqChannel = MqConnection.CreateModel();
        }

        //private void HandleMqConnectionShutdown(IConnection connection, ShutdownEventArgs reason)
        //{
        //    Log.Warning(
        //        "{RabbitSubscriberHostType} connection 'ShutDown' event received, reason is {RabbitShutDownArgs}",
        //        GetType().Name,
        //        reason);
        //}

        private void CreateSubscription()
        {
            _mqConsumerTag = MqChannel.BasicConsume(_subscriberConfig.Queue, false, _consumer);
        }

    }
}
