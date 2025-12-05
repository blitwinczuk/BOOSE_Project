using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSE;
using BOOSEapp;

namespace BOOSEappTests
{
    [TestClass]
    public class CanvasCommandTests
    {
        private AppCanvas _canvas;
        private AppCommandFactory _factory;
        private StoredProgram _program;
        private IParser _parser;

        [TestInitialize]
        public void Setup()
        {
            // Use any sensible canvas size
            _canvas = new AppCanvas(800, 600);
            _factory = new AppCommandFactory(_canvas);
            _program = new StoredProgram(_canvas);
            _parser = new Parser(_factory, _program);
        }

        // 1) Unit Test for moveto command (variables storing pen position are correct)
        [TestMethod]
        public void MoveToCommand_UpdatesPenPositionVariables()
        {
            // Arrange
            string source = "moveto 150,200";

            // Act
            _parser.ParseProgram(source);
            _program.Run();

            // Assert
            Assert.AreEqual(150, _canvas.Xpos, "X position should be 150 after moveto.");
            Assert.AreEqual(200, _canvas.Ypos, "Y position should be 200 after moveto.");
        }

        // 2) Unit Test for drawto command (variables storing pen position are correct)
        [TestMethod]
        public void DrawToCommand_UpdatesPenPositionVariables()
        {
            // Arrange
            string source =
                "moveto 10,20\r\n" +
                "drawto 100,200";

            // Act
            _parser.ParseProgram(source);
            _program.Run();

            // Assert
            Assert.AreEqual(100, _canvas.Xpos, "X position should be 100 after drawto.");
            Assert.AreEqual(200, _canvas.Ypos, "Y position should be 200 after drawto.");
        }

        // 3) Unit Test for a multiline program
        [TestMethod]
        public void MultilineProgram_ExecutesAndLeavesPenAtExpectedPosition()
        {
            // Uses several commands in sequence and checks final pen position.
            // Command names follow your existing .boose examples (moveto, circle, drawto, etc.).
            string source =
                "moveto 50,50\r\n" +
                "circle 20\r\n" +
                "drawto 120,130\r\n" +
                "moveto 200,210\r\n";

            // Act
            _parser.ParseProgram(source);
            _program.Run();

            // Assert: final command is moveto 200,210, so pen should end there.
            Assert.AreEqual(200, _canvas.Xpos, "Final X position should be 200 after multiline program.");
            Assert.AreEqual(210, _canvas.Ypos, "Final Y position should be 210 after multiline program.");
        }
    }
}
