using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOSEapp
{
    internal class Rectangle : CommandTwoParameters
    {
        private int width;
        private int height;

    public Rectangle() : base()
        {
        }
        public Rectangle(Canvas c, int width, int height) : base(c)
        {
            this.width = width;
            this.height = height;
        }
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
