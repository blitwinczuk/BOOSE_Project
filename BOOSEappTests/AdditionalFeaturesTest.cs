using BOOSE;
using BOOSEapp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Array = BOOSEapp.Array;

namespace BOOSEappTests
{
    /// <summary>
    /// Unit tests for custom BOOSE additional features.
    /// Covers random values, named colours, and filled shapes.
    /// </summary>
    [TestClass]
    public class AdditionalFeaturesTests
    {
        /// <summary>
        /// Tests that additional features such as random, colour,
        /// and fill execute successfully without throwing an exception.
        /// </summary>
        [TestMethod]
        public void AdditionalFeatures_RandomColourFill_ProgramRunsSuccessfully()
        {
            AppCanvas canvas = new AppCanvas(800, 600);
            Array interpreter = new Array(canvas);

            string program = @"
                            int x = 0
                            random x 20 60
                            colour red
                            moveto 100,100
                            rect x,x fill
                            ";

            interpreter.Run(program);

            Assert.IsTrue(true);
        }
    }
}
