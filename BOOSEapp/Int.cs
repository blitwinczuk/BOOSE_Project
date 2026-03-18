using System;
using System.Collections.Generic;

namespace BOOSEapp
{
    /// <summary>
    /// Simple interpreter extension that adds support for integer variables (int).
    /// Allows declaration, assignment and usage of variables in BOOSE commands.
    /// </summary>
    public class Int
    {
        private AppCanvas canvas;
        private Dictionary<string, int> vars = new Dictionary<string, int>();
        private Maths maths;

        /// <summary>
        /// Initializes the interpreter with a drawing canvas.
        /// </summary>
        /// <param name="canvas">Canvas used for drawing operations.</param>
        public Int(AppCanvas canvas)
        {
            this.canvas = canvas;
            maths = new Maths(vars, null);
        }

        /// <summary>
        /// Executes a BOOSE program with integer variable support.
        /// </summary>
        /// <param name="program">Program text entered by the user.</param>
        public void Run(string program)
        {
            string[] lines = program.Split('\n');

            foreach (string raw in lines)
            {
                string line = raw.Trim().ToLower();

                // int declaration
                if (line.StartsWith("int"))
                {
                    string[] p = line.Split(' ');
                    string name = p[1];

                    if (line.Contains("="))
                    {
                        string valueStr = line.Split('=')[1].Trim();
                        int value = maths.EvaluateInt(valueStr);
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

                    int value = maths.EvaluateInt(valueStr);

                    vars[name] = value;
                }

                // circle
                else if (line.StartsWith("circle"))
                {
                    string val = line.Substring(7).Trim();
                    int r = maths.EvaluateInt(val);
                    canvas.Circle(r, false);
                }

                // moveto
                else if (line.StartsWith("moveto"))
                {
                    string[] p = line.Substring(7).Split(',');

                    string xStr = p[0].Trim();
                    string yStr = p[1].Trim();

                    int x = maths.EvaluateInt(xStr);
                    int y = maths.EvaluateInt(yStr);

                    canvas.MoveTo(x, y);
                }
                // rectangle
                else if (line.StartsWith("rect"))
                {
                    string[] p = line.Substring(5).Split(',');

                    string wStr = p[0].Trim();
                    string hStr = p[1].Trim();

                    int w = maths.EvaluateInt(wStr);
                    int h = maths.EvaluateInt(hStr);

                    canvas.Rect(w, h, false);
                }

                // pen colour
                else if (line.StartsWith("pen"))
                {
                    string[] p = line.Substring(4).Split(',');

                    string rStr = p[0].Trim();
                    string gStr = p[1].Trim();
                    string bStr = p[2].Trim();

                    int r = maths.EvaluateInt(rStr);
                    int g = maths.EvaluateInt(gStr);
                    int b = maths.EvaluateInt(bStr);

                    canvas.SetColour(r, g, b);
                }
                //write
                else if (line.StartsWith("write"))
                {
                    string expr = line.Substring(6).Trim();
                    double result = maths.EvaluateReal(expr);
                    canvas.WriteText(result.ToString());
                }
            }
        }
    }
}