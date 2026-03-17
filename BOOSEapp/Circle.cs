using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// BOOSE command that draws a circle on the canvas.
    /// </summary>
    internal class Circle : CommandOneParameter
    {
        private int radius;

        /// <summary>
        /// Default constructor used by the command factory.
        /// </summary>
        public Circle() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Circle command.
        /// </summary>
        /// <param name="c">Canvas used for drawing.</param>
        /// <param name="radius">Radius of the circle.</param>
        public Circle(Canvas c, int radius) : base(c)
        {
            this.radius = radius;
        }

        /// <summary>
        /// Executes the circle command and draws the circle.
        /// </summary>
        /// <exception cref="CanvasException">
        /// Thrown when the radius parameter is invalid.
        /// </exception>
        public override void Execute()
        {
            base.Execute();

            if (IsDouble)
                throw new CanvasException("Circle command requires an integer radius.");

            radius = Paramsint[0];

            if (radius < 1)
                throw new CanvasException("Radius must be a positive integer.");

            Canvas.Circle(radius, false);
        }
    }
}