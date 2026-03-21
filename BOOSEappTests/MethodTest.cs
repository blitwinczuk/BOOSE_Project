using BOOSE;
using BOOSEapp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Method = BOOSEapp.Method;

namespace BOOSEappTests
{
    /// <summary>
    /// Unit tests for BOOSE method features.
    /// Covers method definition, parameter passing, and method calls.
    /// </summary>
    [TestClass]
    public class MethodsTests
    {
        /// <summary>
        /// Tests support for method definition, method calls, and parameter passing.
        /// The test passes if the program executes without throwing an exception.
        /// </summary>
        [TestMethod]
        public void Methods_CallAndExecution_ProgramRunsSuccessfully()
        {
            AppCanvas canvas = new AppCanvas(800, 600);
            Method interpreter = new Method(canvas);

            string program = @"
                            method int addMethod int one, int two
                                addMethod = one + two
                            end method

                            method int mulMethod int one, int two
                                mulMethod = one * two
                            end method

                            call addMethod 5 7
                            call mulMethod 4 3
                            write addMethod
                            write mulMethod
                            ";

            interpreter.Run(program);

            Assert.IsTrue(true);
        }
    }
}
