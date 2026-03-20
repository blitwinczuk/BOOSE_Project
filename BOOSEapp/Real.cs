using System;
using System.Collections.Generic;

namespace BOOSEapp
{
    /// <summary>
    /// Real variable support for BOOSE interpreter.
    /// Allows declaration, assignment and usage of variables in BOOSE commands.
    /// </summary>
    public class Real : IInterpreter
    {
        private AppCanvas canvas;
        private Dictionary<string, double> vars = new Dictionary<string, double>();
        private Maths maths;

        /// <summary>
        /// Initializes the interpreter with a drawing canvas.
        /// </summary>
        /// <param name="canvas">Canvas used for drawing operations.</param>
        public Real(AppCanvas canvas)
        {
            this.canvas = canvas;
            maths = new Maths(null, vars);
        }

        /// <summary>
        /// Executes a BOOSE program with real variable support.
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
                        double value = maths.EvaluateReal(valueStr);
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

                    double value = maths.EvaluateReal(valueStr);
                    vars[name] = value;
                }

                // circle
                else if (line.StartsWith("circle"))
                {
                    string val = line.Substring(7).Trim();
                    int r = (int)maths.EvaluateReal(val);
                    canvas.Circle(r, false);
                }

                // moveto
                else if (line.StartsWith("moveto"))
                {
                    string[] p = line.Substring(7).Split(',');

                    string xStr = p[0].Trim();
                    string yStr = p[1].Trim();

                    int x = (int)maths.EvaluateReal(xStr);
                    int y = (int)maths.EvaluateReal(yStr);

                    canvas.MoveTo(x, y);
                }

                // rect
                else if (line.StartsWith("rect"))
                {
                    string[] p = line.Substring(5).Split(',');

                    string wStr = p[0].Trim();
                    string hStr = p[1].Trim();

                    int w = (int)maths.EvaluateReal(wStr);
                    int h = (int)maths.EvaluateReal(hStr);

                    canvas.Rect(w, h, false);
                }

                // pen
                else if (line.StartsWith("pen"))
                {
                    string[] p = line.Substring(4).Split(',');

                    string rStr = p[0].Trim();
                    string gStr = p[1].Trim();
                    string bStr = p[2].Trim();

                    int r = (int)maths.EvaluateReal(rStr);
                    int g = (int)maths.EvaluateReal(gStr);
                    int b = (int)maths.EvaluateReal(bStr);

                    canvas.SetColour(r, g, b);
                }

                // write
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