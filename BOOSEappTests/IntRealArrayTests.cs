using BOOSE;
using BOOSEapp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Array = BOOSEapp.Array;

namespace BOOSEappTests
{
    /// <summary>
    /// Unit tests for BOOSE variable features.
    /// Covers int, real, and array support.
    /// </summary>
    [TestClass]
    public class VariablesTests
    {
        /// <summary>
        /// Tests support for integer variables, real variables, and arrays.
        /// The test passes if the program executes without throwing an exception.
        /// </summary>
        [TestMethod]
        public void Variables_IntRealArray_ProgramRunsSuccessfully()
        {
            AppCanvas canvas = new AppCanvas(800, 600);
            Array interpreter = new Array(canvas);

            string program = @"
                            int a = 10
                            real b = 5.5
                            array int nums 3
                            array real vals 2
                            poke nums 0 = a
                            poke nums 1 = 20
                            poke nums 2 = 30
                            poke vals 0 = b
                            poke vals 1 = 2.5
                            peek a = nums 1
                            peek b = vals 0
                            write a
                            write b
                            ";

            interpreter.Run(program);

            Assert.IsTrue(true);
        }
    }
}
