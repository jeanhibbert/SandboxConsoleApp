using System;
using System.Collections.Generic;

namespace SandboxConsoleApp.LenientKing
{
    public class PrisonerGenerator
    {
        public PrisonerGenerator()
        {
        }

        public List<Prisoner> GeneratePrisoners(int totalPrisoners)
        {
            List<Prisoner> prisoners = new List<Prisoner>();
            for (int i = 0; i < totalPrisoners; i++)
            {
                prisoners.Add(new Prisoner { IsAlive = true, Number = i + 1 });
            }
            return prisoners;
        }
    }
}