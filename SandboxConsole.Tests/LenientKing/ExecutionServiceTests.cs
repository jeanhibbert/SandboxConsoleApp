using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SandboxConsoleApp.LenientKing;

namespace SandboxConsole.Tests.LenientKing
{
    //Lines all prisoners up in a circle
    //Prisoner total 1 => infinity (prisoners tagged with number)
    //Sings a rhyme with sylabyls 1 => infinity and executes every X prisoner 
    //Need to know the number of the last prisoner standing

    [TestClass]
    public class ExecutionServiceTests
    {
        private ExecutionService _executionService = new ExecutionService(new PrisonerGenerator());

        [TestMethod]
        public void CanGeneratePrisoners()
        {
            PrisonerGenerator prisonerGenerator = new PrisonerGenerator();
            List<Prisoner> prisoners = prisonerGenerator.GeneratePrisoners(10);

            Assert.IsNotNull(prisoners);
            Assert.IsTrue(prisoners.Any());
            Assert.IsTrue(prisoners.All(x => x.Number > 0));
            Assert.IsTrue(prisoners.All(x => x.IsAlive));
        }

        [TestMethod]
        public void CanExecutePrisoner()
        {
            Prisoner lastPrisoner = _executionService.Execute(3, 10);

            Assert.IsNotNull(lastPrisoner);
        }

        [TestMethod]
        public void CanExecute_3_Prisoners_2_sylabyls()
        {
            Prisoner lastPrisoner = _executionService.Execute(2, 3);

            Assert.IsNotNull(lastPrisoner);
            Assert.AreEqual(lastPrisoner.Number, 3);
        }

        [TestMethod]
        public void CanExecute_3_Prisoners_5_sylabyls()
        {
            Prisoner lastPrisoner = _executionService.Execute(5, 3);

            Assert.IsNotNull(lastPrisoner);
            Assert.AreEqual(lastPrisoner.Number, 1);
        }

        [TestMethod]
        public void CanExecute_5_Prisoners_5_sylabyls()
        {
            Prisoner lastPrisoner = _executionService.Execute(5, 5);

            Assert.IsNotNull(lastPrisoner);
            Assert.AreEqual(lastPrisoner.Number, 2);
        }

        [TestMethod]
        public void CanNotExecute_1_Prisoners_5_sylabyls()
        {
            Prisoner lastPrisoner = _executionService.Execute(5, 1);

            Assert.IsNotNull(lastPrisoner);
            Assert.AreEqual(lastPrisoner.Number, 1);
        }

        [TestMethod]
        public void CanExecute_10_Prisoners_2_sylabyls()
        {
            Prisoner lastPrisoner = _executionService.Execute(2, 10);

            Assert.IsNotNull(lastPrisoner);
            Assert.AreEqual(lastPrisoner.Number, 5);
        }

        [TestMethod]
        public void CanNotExecute_1_Prisoners_1_sylabyls()
        {
            Prisoner lastPrisoner = _executionService.Execute(1, 1);

            Assert.IsNotNull(lastPrisoner);
            Assert.AreEqual(lastPrisoner.Number, 1);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WillThrowExceptionWhenAttemptingToExecuteZeroPrisoners()
        {
            Prisoner lastPrisoner = _executionService.Execute(5, 0);
        }
    }
}
