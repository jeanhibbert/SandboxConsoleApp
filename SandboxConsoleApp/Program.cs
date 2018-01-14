using SandboxConsoleApp.Dynamic;
using SandboxConsoleApp.Http;
using System;

namespace SandboxConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //ScriptGenerator.GenerateScript();
            //BlockChainGenerator.BuildBlockChain();

            HttpClientTests.TestHttpClient();

            Console.ReadKey();
        }
    }
}
