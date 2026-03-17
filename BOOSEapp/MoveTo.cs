
using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// BOOSE command that moves the drawing cursor
    /// to a specified position on the canvas.
    /// </summary>
    internal class MoveTo : CommandTwoParameters
    {
        private int x;
        private int y;

        /// <summary>
        /// Default constructor used by the command factory.
        /// </summary>
        public MoveTo() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MoveTo command.
        /// </summary>
        /// <param name="c">Canvas used for drawing.</param>
        /// <param name="x">Target X coordinate.</param>
        /// <param name="y">Target Y coordinate.</param>
        public MoveTo(Canvas c, int x, int y) : base(c)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Executes the MoveTo command and moves the drawing cursor.
        /// </summary>
        /// <exception cref="CanvasException">
        /// Thrown when parameters are invalid.
        /// </exception>
        public override void Execute()
        {
            base.Execute();

            if (IsDouble)
                throw new CanvasException("MoveTo command requires two integer parameters.");

            x = Paramsint[0];
            y = Paramsint[1];

            if (x < 0 || y < 0)
                throw new CanvasException("X and Y coordinates must be non-negative integers.");

            Canvas.MoveTo(x, y);
        }
    }
}