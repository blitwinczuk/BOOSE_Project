using System.Collections.Generic;
using System.Globalization;
using System;

namespace BOOSEapp
{
    /// <summary>
    /// Array interpreter extension for BOOSE, adding support for array variables.
    /// Supports int variables, real variables, int arrays, real arrays,
    /// together with poke, peek and simple write commands.
    /// </summary>
    public class Array
    {

        private AppCanvas canvas;
        private Dictionary<string, int> intVars = new Dictionary<string, int>();
        private Dictionary<string, double> realVars = new Dictionary<string, double>();
        private Dictionary<string, int[]> intArrays = new Dictionary<string, int[]>();
        private Dictionary<string, double[]> realArrays = new Dictionary<string, double[]>();
        private Maths maths;
        /// <summary>
        /// Initializes the array interpreter with a drawing canvas.
        /// </summary>
        /// <param name="canvas">Canvas used for drawing operations.</param>
        public Array(AppCanvas canvas)
        {
            this.canvas = canvas;
            maths = new Maths(intVars, realVars);
        }


    }   
}
