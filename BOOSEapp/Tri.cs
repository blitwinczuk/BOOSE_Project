using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// BOOSE command that draws a triangle on the canvas.
    /// </summary>
    internal class Tri : CommandTwoParameters
    {
        private int width;
        private int height;

        public Tri() : base()
        {
        }

        public Tri(Canvas c, int width, int height) : base(c)
        {
            this.width = width;
            this.height = height;
        }

        public override void Execute()
        {
            base.Execute();

            if (IsDouble)
                throw new CanvasException("Tri command requires two integer parameters.");

            width = Paramsint[0];
            height = Paramsint[1];

            if (width < 1 || height < 1)
                throw new CanvasException("Width and height must be positive integers.");

            Canvas.Tri(width, height);
        }
    }
}