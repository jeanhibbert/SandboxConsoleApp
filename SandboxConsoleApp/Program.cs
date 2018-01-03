using SandboxConsoleApp.Dynamic;
using System;

namespace SandboxConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ScriptGenerator.GenerateScript();
            Console.ReadKey();
        }
    }
}
