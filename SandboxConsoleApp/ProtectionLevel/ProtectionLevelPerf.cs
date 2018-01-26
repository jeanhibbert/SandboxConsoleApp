using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SandboxConsoleApp.ProtectionLevel
{
    class ProtectionLevelPerf
    {
        // Setup the Service host
        private static ServiceHost StartServer<T>(string Uri, NetTcpBinding binding)
        {
            Uri uri = new Uri(Uri);

            ServiceHost host = new ServiceHost(typeof(ConvertStringService));
            host.AddServiceEndpoint(typeof(T), binding, uri);
            host.Open();

            Console.WriteLine("Service opened using NetTcpBinding with {0} protection level.", Enum.GetName(typeof(System.Net.Security.ProtectionLevel), binding.Security.Transport.ProtectionLevel));

            return host;
        }

        // Create channel using NetTcp binding with Uri and execute performance test.
        private static double RunPerformanceTestOnNetTcpBinding(NetTcpBinding binding, string endpointUri)
        {
            const int iterations = 300;
            const string stringToConvert = "12";

            binding.Security.Mode = SecurityMode.Transport;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (Int32 i = 0; i < iterations; i++)
            {
                ChannelFactory<IConvertString>.CreateChannel(binding,
                    new EndpointAddress(endpointUri))
                    .ConvertString(stringToConvert);
            }
            watch.Stop();

            Console.WriteLine("ConvertString with ProtectionLevel {2} : {0} times; Time taken : {1} ",
                iterations.ToString(),
                watch.Elapsed.TotalSeconds.ToString(),
                Enum.GetName(typeof(System.Net.Security.ProtectionLevel), binding.Security.Transport.ProtectionLevel));

            return watch.Elapsed.TotalSeconds;

        }

        public static void RunPerTests(string[] args)
        {

            string uri = "net.tcp://localhost:7000/IConvertString";

            NetTcpBinding bindingNoEncryption = new NetTcpBinding();
            bindingNoEncryption.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

            NetTcpBinding bindingWithSign = new NetTcpBinding();
            bindingWithSign.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;

            NetTcpBinding bindingWithEncryption = new NetTcpBinding();
            bindingWithEncryption.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            Console.WriteLine("Run performance test or press q to Quit...");
            while (Console.ReadLine() != "q")
            {

                double noEncryptionTime;
                double signTime;
                double encryptionTime;

                using (ServiceHost host = StartServer<IConvertString>(uri, bindingNoEncryption))
                {

                    noEncryptionTime = RunPerformanceTestOnNetTcpBinding(bindingNoEncryption, uri);
                    host.Close();
                }

                using (ServiceHost host = StartServer<IConvertString>(uri, bindingWithSign))
                {
                    signTime = RunPerformanceTestOnNetTcpBinding(bindingWithSign, uri);
                    host.Close();
                }

                using (ServiceHost host = StartServer<IConvertString>(uri, bindingWithEncryption))
                {
                    encryptionTime = RunPerformanceTestOnNetTcpBinding(bindingWithEncryption, uri);
                    host.Close();
                }

                Console.WriteLine("Run performance test again or press 'q' to Quit...");

            }

        }

    }

 
}
