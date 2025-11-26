
using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOSEapp
{
    internal class Circle : CommandOneParameter
    {
        private int radius;

        public Circle() : base()
        {
        }

        public Circle(Canvas c, int radius) : base(c)
        {
            this.radius = radius;
        }

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
