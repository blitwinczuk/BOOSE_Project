
using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOSEapp
{
    internal class MoveTo : CommandTwoParameters
    {
        private int x;
        private int y;
        public MoveTo() : base()
        {
        }
        public MoveTo(Canvas c, int x, int y) : base(c)
        {
            this.x = x;
            this.y = y;
        }
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
