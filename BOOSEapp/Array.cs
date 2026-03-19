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
        // Boolean 
        private Dictionary<string, bool> boolVars = new Dictionary<string, bool>();
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

        /// <summary>
        /// Executes a BOOSE program with array support.
        /// </summary>
        /// <param name="program">Program text entered by the user.</param>
        public void Run(string program)
        {
            string[] lines = program.Split('\n');

            foreach (string raw in lines)
            {
                string line = raw.Trim();

                if (line == "")
                    continue;

                string lower = line.ToLower();

                // int support
                if (lower.StartsWith("int "))
                {
                    string[] p = line.Split(' ');
                    string name = p[1];

                    if (line.Contains("="))
                    {
                        string valueStr = line.Split('=')[1].Trim();
                        intVars[name] = maths.EvaluateInt(valueStr);
                    }
                    else
                    {
                        intVars[name] = 0;
                    }
                }

                // real support
                else if (lower.StartsWith("real "))
                {
                    string[] p = line.Split(' ');
                    string name = p[1];

                    if (line.Contains("="))
                    {
                        string valueStr = line.Split('=')[1].Trim();
                        realVars[name] = maths.EvaluateReal(valueStr);
                    }
                    else
                    {
                        realVars[name] = 0;
                    }
                }

                // array + int
                else if (lower.StartsWith("array int "))
                {

                    string[] p = line.Split(' ');
                    string name = p[2];
                    int size = int.Parse(p[3]);

                    // only create array if it doesn't exist, to allow reusing same name for multiple declarations without losing data (AI suggestion)
                    if (!intArrays.ContainsKey(name))
                    {
                        intArrays[name] = new int[size];
                    }
                }

                // real + int
                else if (lower.StartsWith("array real "))
                {

                    string[] p = line.Split(' ');
                    string name = p[2];
                    int size = int.Parse(p[3]);

                    // only create array if it doesn't exist, to allow reusing same name for multiple declarations without losing data (AI suggestion)
                    if (!realArrays.ContainsKey(name))
                    {
                        realArrays[name] = new double[size];
                    }
                }

                // poke - first poke was my own, then I added support for array names that are substrings of each other after AI suggestion, which is why FindIntArrayName and FindRealArrayName methods were added
                else if (lower.StartsWith("poke "))
                {
                    string[] leftRight = line.Split('=');

                    string left = leftRight[0].Trim();
                    string valueStr = leftRight[1].Trim();

                    string[] p = left.Split(' ');
                    string arrayName = p[1];
                    int index = maths.EvaluateInt(p[2]);

                    string intName = FindIntArrayName(arrayName);
                    string realName = FindRealArrayName(arrayName);

                    if (intName != null)
                    {
                        intArrays[intName][index] = maths.EvaluateInt(valueStr);
                    }
                    else if (realName != null)
                    {
                        realArrays[realName][index] = maths.EvaluateReal(valueStr);
                    }
                }

                // peek - first peek was my own, then I added support for array names that are substrings of each other after AI suggestion, which is why FindIntArrayName and FindRealArrayName methods were added
                else if (lower.StartsWith("peek "))
                {
                    string[] leftRight = line.Split('=');

                    string left = leftRight[0].Trim();
                    string right = leftRight[1].Trim();

                    string varName = left.Split(' ')[1];

                    string[] p = right.Split(' ');
                    string arrayName = p[0];
                    int index = maths.EvaluateInt(p[1]);

                    string intName = FindIntArrayName(arrayName);
                    string realName = FindRealArrayName(arrayName);

                    if (intName != null && intVars.ContainsKey(varName))
                    {
                        intVars[varName] = intArrays[intName][index];
                    }
                    else if (realName != null && realVars.ContainsKey(varName))
                    {
                        realVars[varName] = realArrays[realName][index];
                    }
                }

                // boolean support
                else if (lower.StartsWith("boolean "))
                {
                    string[] parts = line.Split('=');

                    string left = parts[0].Trim();          
                    string[] leftParts = left.Split(' ');

                    string name = leftParts[1].Trim().ToLower();

                    if (line.Contains("="))
                    {
                        string valueStr = parts[1].Trim();
                        boolVars[name] = EvaluateBoolean(valueStr);
                    }
                    else
                    {
                        boolVars[name] = false;
                    }
                }



                else if (line.Contains("="))
                {
                    string[] p = line.Split('=');

                    string name = p[0].Trim().ToLower();
                    string valueStr = p[1].Trim();

                    if (intVars.ContainsKey(name))
                    {
                        intVars[name] = maths.EvaluateInt(valueStr);
                    }
                    else if (realVars.ContainsKey(name))
                    {
                        realVars[name] = maths.EvaluateReal(valueStr);
                    }
                    else if (boolVars.ContainsKey(name))
                    {
                        boolVars[name] = EvaluateBoolean(valueStr);
                    }
                }

                // moveto
                else if (lower.StartsWith("moveto "))
                {
                    string[] p = line.Substring(7).Split(',');

                    int x = maths.EvaluateInt(p[0]);
                    int y = maths.EvaluateInt(p[1]);

                    canvas.MoveTo(x, y);
                }

                // circle
                else if (lower.StartsWith("circle "))
                {
                    string value = line.Substring(7).Trim();
                    int r = maths.EvaluateInt(value);

                    canvas.Circle(r, false);
                }

                // rect
                else if (lower.StartsWith("rect "))
                {
                    string[] p = line.Substring(5).Split(',');

                    int w = maths.EvaluateInt(p[0]);
                    int h = maths.EvaluateInt(p[1]);

                    canvas.Rect(w, h, false);
                }

                // pen
                else if (lower.StartsWith("pen "))
                {
                    string[] p = line.Substring(4).Split(',');

                    int r = maths.EvaluateInt(p[0]);
                    int g = maths.EvaluateInt(p[1]);
                    int b = maths.EvaluateInt(p[2]);

                    canvas.SetColour(r, g, b);
                }

                // write
                else if (lower.StartsWith("write "))
                {
                    string expr = line.Substring(6).Trim();

                    // boolean variable
                    string boolKey = expr.ToLower();
                    if (boolVars.ContainsKey(boolKey))
                    {
                        canvas.WriteText(boolVars[boolKey].ToString().ToLower());
                    }
                    else
                    {
                        string text = BuildWriteText(expr);
                        canvas.WriteText(text);
                    }
                }
                
            }
        }

        /// <summary>
        /// Builds text for the write command.
        /// Supports:
        /// write x
        /// write y
        /// write "hello world!"
        /// write "" + x/y/z
        /// </summary>
        /// <param name="expr">Expression after write.</param>
        /// <returns>Text to display.</returns>
        private string BuildWriteText(string expr)
        {
            expr = expr.Trim();

            // text + variable
            if (expr.Contains("+"))
            {
                string[] p = expr.Split('+');

                string left = p[0].Trim();
                string right = p[1].Trim();

                string leftText = GetWriteValue(left);
                string rightText = GetWriteValue(right);

                return leftText + rightText;
            }

            // single value
            return GetWriteValue(expr);
        }

        /// <summary>
        /// Resolves value for write command.
        /// Can return string literal, int variable, real variable, boolean variable or plain text.
        /// </summary>
        /// <param name="value">Input text or variable name.</param>
        /// <returns>Resolved text.</returns>
        private string GetWriteValue(string value)
        {
            value = value.Trim();
            string key = value.ToLower();

            // string literal
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                return value.Substring(1, value.Length - 2);
            }

            // int variable
            if (intVars.ContainsKey(key))
            {
                return intVars[key].ToString();
            }

            // real variable
            if (realVars.ContainsKey(key))
            {
                return realVars[key].ToString(CultureInfo.InvariantCulture);
            }

            // boolean variable
            if (boolVars.ContainsKey(key))
            {
                return boolVars[key].ToString().ToLower();
            }

            return value;
        }

        ///<summary>Methods that AI suggested to help out with unrestricet issue</summary>
        ///<param name="name">Name of variable/array to find</param>"
        private string FindIntArrayName(string name)
        {
            if (intArrays.ContainsKey(name))
                return name;

            foreach (string key in intArrays.Keys)
            {
                if (key.StartsWith(name) || name.StartsWith(key))
                    return key;
            }

            return null;
        }

        private string FindRealArrayName(string name)
        {
            if (realArrays.ContainsKey(name))
                return name;

            foreach (string key in realArrays.Keys)
            {
                if (key.StartsWith(name) || name.StartsWith(key))
                    return key;
            }

            return null;
        }

        /// <summary>
        /// Evaluates boolean expressions like:
        /// true, false
        /// </summary>
        private bool EvaluateBoolean(string expr)
        {
            expr = expr.Trim().ToLower();

            if (expr == "true")
                return true;

            if (expr == "false")
                return false;

            if (expr.Contains("&&"))
            {
                string[] p = expr.Split(new string[] { "&&" }, StringSplitOptions.None);
                bool left = EvaluateBoolean(p[0]);
                bool right = EvaluateBoolean(p[1]);
                return left && right;
            }

            if (boolVars.ContainsKey(expr))
                return boolVars[expr];

            throw new Exception("Unknown boolean expression: " + expr);
        }
    }
}   

