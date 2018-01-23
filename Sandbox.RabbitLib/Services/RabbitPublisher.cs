using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::RabbitMQ.Client;
using Sandbox.Core;
using Sandbox.RabbitLib.Interfaces;
using Sandbox.RabbitLib.Model;

namespace Sandbox.RabbitLib.Services
{
    public class RabbitPublisher<TArgs> where TArgs : IActivityMonitorEntity
    {
        private IConnection _mqConnection;

        private IModel _mqChannel;

        private IRabbitPublisherConfiguration _mqConfig;

        public RabbitPublisher(IRabbitPublisherConfiguration config)
        {
            _mqConfig = config;
            CreateConnectionAndChannel();
        }

        private void CreateConnectionAndChannel()
        {

            var factory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = false,
                RequestedHeartbeat = 30   // seconds
            };

            factory.InjectFrom(_mqConfig);

            _mqConnection = factory.CreateConnection();
            _mqChannel = _mqConnection.CreateModel();
        }

        private IRabbitMessage MessageConverter(TArgs args)
        {
            return RabbitMessage.WithMessageId(args.ActivityId)
                .PayloadIsJsonOf(args);
        }

        public RabbitMessage WithMessageId(Guid id)
        {
            return new RabbitMessage { MessageId = id };
        }

        public bool Publish(TArgs cmdArgs)
        {
            try
            {
                var msg = MessageConverter(cmdArgs);

                var props = _mqChannel.CreateBasicProperties();
                props.Persistent = true;
                props.Headers = msg.Headers;

                _mqChannel.BasicPublish(
                    _mqConfig.Exchange,
                    _mqConfig.RoutingKey,
                    props,
                    msg.Payload);

                //if (_publishedCallback != null)
                //{
                //    _publishedCallback(cmdArgs, msg);
                //}

                return true;
            }
            catch (Exception ex)
            {
                //Log.Debug(
                //    ex,
                //    "Failed attempting to publish {@RabbitMessageToPublish}...message will remain in internal queue for retry/recovery.",
                //    cmdArgs);
                return false;
            }
        }
    }


}
