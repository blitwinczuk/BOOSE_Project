using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSE;
using BOOSEapp;

namespace BOOSEappTests
{
    /// <summary>
    /// Unit tests for BOOSE canvas commands and interpreter execution.
    /// </summary>
    [TestClass]
    public class CanvasCommandTests
    {
        private AppCanvas _canvas;
        private AppCommandFactory _factory;
        private StoredProgram _program;
        private IParser _parser;

        /// <summary>
        /// Initializes test environment before each test case.
        /// Sets up canvas, factory, stored program and parser.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _canvas = new AppCanvas(800, 600);
            _factory = new AppCommandFactory(_canvas);
            _program = new StoredProgram(_canvas);
            _parser = new Parser(_factory, _program);
        }

        /// <summary>
        /// Tests that the MoveTo command correctly updates
        /// the pen position variables (Xpos and Ypos).
        /// </summary>
        [TestMethod]
        public void MoveToCommand_UpdatesPenPositionVariables()
        {
            string source = "moveto 150,200";

            _parser.ParseProgram(source);
            _program.Run();

            Assert.AreEqual(150, _canvas.Xpos, "X position should be 150 after moveto.");
            Assert.AreEqual(200, _canvas.Ypos, "Y position should be 200 after moveto.");
        }

        /// <summary>
        /// Tests that the DrawTo command correctly updates
        /// the pen position variables after drawing a line.
        /// </summary>
        [TestMethod]
        public void DrawToCommand_UpdatesPenPositionVariables()
        {
            string source =
                "moveto 10,20\r\n" +
                "drawto 100,200";

            _parser.ParseProgram(source);
            _program.Run();

            Assert.AreEqual(100, _canvas.Xpos, "X position should be 100 after drawto.");
            Assert.AreEqual(200, _canvas.Ypos, "Y position should be 200 after drawto.");
        }

        /// <summary>
        /// Tests execution of a multiline BOOSE program
        /// and verifies that the final pen position is correct.
        /// </summary>
        [TestMethod]
        public void MultilineProgram_ExecutesAndLeavesPenAtExpectedPosition()
        {
            string source =
                "moveto 50,50\r\n" +
                "circle 20\r\n" +
                "drawto 120,130\r\n" +
                "moveto 200,210\r\n";

            _parser.ParseProgram(source);
            _program.Run();

            Assert.AreEqual(200, _canvas.Xpos, "Final X position should be 200 after multiline program.");
            Assert.AreEqual(210, _canvas.Ypos, "Final Y position should be 210 after multiline program.");
        }
    }
}
