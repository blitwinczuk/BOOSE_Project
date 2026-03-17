using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// BOOSE command that draws a rectangle on the canvas.
    /// </summary>
    internal class Rectangle : CommandTwoParameters
    {
        private int width;
        private int height;

        /// <summary>
        /// Default constructor used by the command factory.
        /// </summary>
        public Rectangle() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Rectangle command.
        /// </summary>
        /// <param name="c">Canvas used for drawing.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        public Rectangle(Canvas c, int width, int height) : base(c)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Executes the rectangle command and draws the rectangle.
        /// </summary>
        /// <exception cref="CanvasException">
        /// Thrown when width or height parameters are invalid.
        /// </exception>
        public override void Execute()
        {
            base.Execute();

            if (IsDouble)
                throw new CanvasException("Rectangle command requires two integer parameters.");

            width = Paramsint[0];
            height = Paramsint[1];

            if (width < 1 || height < 1)
                throw new CanvasException("Width and height must be positive integers.");

            Canvas.Rect(width, height, false);
        }
    }
}