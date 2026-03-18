using System;
using System.Collections.Generic;
using System.Globalization;

namespace BOOSEapp
{
    /// <summary>
    /// Real variable support for the BOOSE interpreter.
    /// Allows declaration, assignment and usage of real variables in BOOSE commands.
    /// </summary>
    public class Real
    {
        private AppCanvas canvas;
        private Dictionary<string, double> vars = new Dictionary<string, double>();

        /// <summary>
        /// Initialises the interpreter with a canvas.
        /// </summary>
        /// <param name="canvas">Canvas used for drawing operations.</param>
        public Real(AppCanvas canvas)
        {
            this.canvas = canvas;
        }

        /// <summary>
        /// Executes a BOOSE program with real var support
        /// </summary>
        /// <param name="program">Program text entered by the user.</param>
        public void Run(string program)
        {
            string[] lines = program.Split('\n');

            foreach (string raw in lines)
            {
                string line = raw.Trim().ToLower();

                // real declaration
                if (line.StartsWith("real"))
                {
                    string[] p = line.Split(' ');
                    string name = p[1];

                    if (line.Contains("="))
                    {
                        string valueStr = line.Split('=')[1].Trim();

                        double value = vars.ContainsKey(valueStr)
                            ? vars[valueStr]
                            : double.Parse(valueStr, CultureInfo.InvariantCulture);

                        vars[name] = value;
                    }
                    else
                    {
                        vars[name] = 0;
                    }
                }

                
                else if (line.Contains("="))
                {
                    string[] p = line.Split('=');

                    string name = p[0].Trim();
                    string valueStr = p[1].Trim();

                    double value = vars.ContainsKey(valueStr)
                        ? vars[valueStr]
                        : double.Parse(valueStr, CultureInfo.InvariantCulture);

                    vars[name] = value;
                }

                // circle
                else if (line.StartsWith("circle"))
                {
                    string val = line.Substring(7).Trim();

                    int r = vars.ContainsKey(val)
                        ? (int)vars[val]
                        : (int)double.Parse(val, CultureInfo.InvariantCulture);

                    canvas.Circle(r, false);
                }

                // moveto
                else if (line.StartsWith("moveto"))
                {
                    string[] p = line.Substring(7).Split(',');

                    string xStr = p[0].Trim();
                    string yStr = p[1].Trim();

                    int x = vars.ContainsKey(xStr)
                        ? (int)vars[xStr]
                        : (int)double.Parse(xStr, CultureInfo.InvariantCulture);

                    int y = vars.ContainsKey(yStr)
                        ? (int)vars[yStr]
                        : (int)double.Parse(yStr, CultureInfo.InvariantCulture);

                    canvas.MoveTo(x, y);
                }

                // rectangle
                else if (line.StartsWith("rect"))
                {
                    string[] p = line.Substring(5).Split(',');

                    string wStr = p[0].Trim();
                    string hStr = p[1].Trim();

                    int w = vars.ContainsKey(wStr)
                        ? (int)vars[wStr]
                        : (int)double.Parse(wStr, CultureInfo.InvariantCulture);

                    int h = vars.ContainsKey(hStr)
                        ? (int)vars[hStr]
                        : (int)double.Parse(hStr, CultureInfo.InvariantCulture);

                    canvas.Rect(w, h, false);
                }

                // pen
                else if (line.StartsWith("pen"))
                {
                    string[] p = line.Substring(4).Split(',');

                    string rStr = p[0].Trim();
                    string gStr = p[1].Trim();
                    string bStr = p[2].Trim();

                    int r = vars.ContainsKey(rStr)
                        ? (int)vars[rStr]
                        : (int)double.Parse(rStr, CultureInfo.InvariantCulture);

                    int g = vars.ContainsKey(gStr)
                        ? (int)vars[gStr]
                        : (int)double.Parse(gStr, CultureInfo.InvariantCulture);

                    int b = vars.ContainsKey(bStr)
                        ? (int)vars[bStr]
                        : (int)double.Parse(bStr, CultureInfo.InvariantCulture);

                    canvas.SetColour(r, g, b);
                }
            }
        }
    }
}