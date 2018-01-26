using System;
using System.ServiceModel;

namespace SandboxConsoleApp.ProtectionLevel
{
    public static class SimpleChannelBuilder
    {
        public static void AssembleBasicServerTest()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

            string uriString = "net.tcp://localhost:7000/IConvertString";
            Uri uri = new Uri(uriString);

            // Host the service
            ServiceHost host = new ServiceHost(typeof(ConvertStringService));
            host.AddServiceEndpoint(typeof(IConvertString), binding, uri); // contract, binding and uri address
            host.Open();

            // Create the channel on the client
            EndpointAddress endpointAddress = new EndpointAddress(uri);
            IConvertString channel = ChannelFactory<IConvertString>.CreateChannel(binding, endpointAddress);
            string convertedString = channel.ConvertString("Jesse James");

            if (host.State != CommunicationState.Closed) { host.Close(); }

            Console.WriteLine($"String converted! {convertedString}");

            Console.ReadKey();
        }
    }
}
