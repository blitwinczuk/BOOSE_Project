using System;
using BOOSE;
using BOOSEapp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Method = BOOSEapp.Method;

namespace BOOSEappTests
{
    /// <summary>
    /// Unit tests for invalid method calls.
    /// </summary>
    [TestClass]
    public class MethodNotFoundTests
    {
        /// <summary>
        /// Tests that an exception is thrown when a method is called
        /// but has not been defined.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Methods_MethodNotFound_ThrowsException()
        {
            AppCanvas canvas = new AppCanvas(800, 600);
            Method interpreter = new Method(canvas);

            string program = @"
                            call unknownMethod 10 5
                            ";

            interpreter.Run(program);
        }
    }
}