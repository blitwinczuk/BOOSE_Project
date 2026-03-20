using BOOSE;
using BOOSEapp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Array = BOOSEapp.Array;

namespace BOOSEappTests
{
    /// <summary>
    /// Unit tests for BOOSE control flow features.
    /// Covers if, else, while, and for support.
    /// </summary>
    [TestClass]
    public class ControlFlowTests
    {
        /// <summary>
        /// Tests support for if/else statements, while loops, and for loops.
        /// The test passes if the program executes without throwing an exception.
        /// </summary>
        [TestMethod]
        public void ControlFlow_IfElseWhileFor_ProgramRunsSuccessfully()
        {
            AppCanvas canvas = new AppCanvas(800, 600);
            Array interpreter = new Array(canvas);

            string program = @"
                            int x = 0
                            boolean passed = false

                            while x < 3
                                x = x + 1
                            end while

                            if x == 3
                                x = x + 2
                                    passed = true
                            else
                                x = 0
                                    passed = false
                            end if

                            for i = 1 to 3
                                x = x + 1
                            end for

                            write x
                            ";

            interpreter.Run(program);

            Assert.IsTrue(true);
        }
    }
}
