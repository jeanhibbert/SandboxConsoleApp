using Sandbox.RabbitLib;
using Sandbox.RabbitLib.Config;
using Sandbox.RabbitLib.Interfaces;
using Sandbox.RabbitLib.Model;
using Sandbox.RabbitLib.Services;
using System;

namespace SandboxConsoleApp.RabbitMq
{
    class RabbitMqTests
    {
        public static void PublishMessageTest()
        {
            IRabbitPublisherConfiguration publishConfig = new RabbitPublisherConfiguration();
            RabbitPublisher<CommandArgs> publisher = new RabbitPublisher<CommandArgs>(publishConfig);

            IRabbitSubscriberConfiguration subscriberConfig = new RabbitSubscriberConfiguration();
            SandboxConsumer consumer = new SandboxConsumer();

            RabbitSubscriber<SandboxConsumer> subscriber = new RabbitSubscriber<SandboxConsumer>(consumer, subscriberConfig);
            subscriber.Start();

            publisher.Publish(new CommandArgs { Value = "some data" });

            

            Console.ReadKey();
        }
    }
}
