using SandboxConsoleApp.CSharp;
using SandboxConsoleApp.Http;
using SandboxConsoleApp.RabbitMq;
using System;
using System.Linq;

namespace SandboxConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //ScriptGenerator.GenerateScript();
            //BlockChainGenerator.BuildBlockChain();

            //HttpClientTests.TestHttpConnector();
            //HttpClientTests.TestAsyncHttpConnector();

            //RabbitMqTests.PublishMessageTest();

            CSharpExercises.GeneralTests();

            //ProtectionLevel.ProtectionLevelPerf.RunPerTests(null);

            //ProtectionLevel.SimpleChannelBuilder.AssembleBasicServerTest();

            Console.ReadKey();
        }
    }
}
