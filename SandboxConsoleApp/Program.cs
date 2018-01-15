using SandboxConsoleApp.Http;
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
            HttpClientTests.TestAsyncHttpConnector();
            

            Console.ReadKey();
        }
    }
}
