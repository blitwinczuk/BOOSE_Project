using System;
using BOOSE;
using BOOSEapp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Method = BOOSEapp.Method;

namespace BOOSEappTests
{
    /// <summary>
    /// Unit tests for invalid method parameter usage.
    /// </summary>
    [TestClass]
    public class WrongParameterTests
    {
        /// <summary>
        /// Tests that an exception is thrown when a method is called
        /// with the wrong number of parameters.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Methods_WrongParameterCount_ThrowsException()
        {
            AppCanvas canvas = new AppCanvas(800, 600);
            Method interpreter = new Method(canvas);

            string program = @"
                            method int testMethod int a, int b
                                testMethod = a + b
                            end method

                            call testMethod 5
                            ";

            interpreter.Run(program);
        }
    }
}
