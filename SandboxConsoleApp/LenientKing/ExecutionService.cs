using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SandboxConsoleApp.LenientKing
{
    public class ExecutionService
    {
        private PrisonerGenerator prisonerGenerator;
        
        public ExecutionService(PrisonerGenerator prisonerGenerator)
        {
            this.prisonerGenerator = prisonerGenerator;
        }
        
        public Prisoner Execute(int sylabylsTotal, int prisonerTotal)
        {
            if (prisonerTotal.Equals(0))
                throw new ArgumentException("prisonerTotal"); // There should always be more than zero prisoners

            var prisonersToExecute = prisonerGenerator.GeneratePrisoners(prisonerTotal);
            if (prisonersToExecute.Count() == 1) return prisonersToExecute[0]; // If there is only 1 prisoner he will always live

            Debug.WriteLine("Starting execution sequence...");
            Prisoner lastPrisoner = null;

            var prisonersLeft = prisonersToExecute.Where(x => x.IsAlive).ToList();
            int sylabylCounter = 0;
            int prisonerCounter = 0;
            while (prisonersLeft.Count > 1)
            {
                sylabylCounter++;

                if(sylabylCounter  % sylabylsTotal == 0)
                {
                    var prisonerToExecute = prisonersLeft[prisonerCounter];
                    prisonerToExecute.IsAlive = false;
                    Debug.WriteLine($"Executed prisoner {prisonerToExecute.Number}");
                }
                
                prisonerCounter++;
                if (prisonerCounter >= prisonersLeft.Count())
                {
                    prisonerCounter = 0;
                    prisonersLeft = prisonersLeft.Where(x => x.IsAlive).ToList();
                    if (prisonersLeft.Count() == 1)
                    {
                        lastPrisoner = prisonersLeft[0];
                        break;
                    }
                }
            }

            return lastPrisoner;
        }
    }
}