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
    public class Array : IInterpreter
    {

        private AppCanvas canvas;
        private Dictionary<string, int> intVars = new() { };
        private Dictionary<string, double> realVars = new() { };
        private Dictionary<string, int[]> intArrays = new() { };
        private Dictionary<string, double[]> realArrays = new() { };
        private Dictionary<string, bool> boolVars = new() { };
        private Maths maths;
        private If ifHandler;
        
        
        /// <summary>
        /// Initializes the array interpreter with a drawing canvas.
        /// </summary>
        /// <param name="canvas">Canvas used for drawing operations.</param>
        public Array(AppCanvas canvas)
        {
            this.canvas = canvas;
            maths = new Maths(intVars, realVars);
            ifHandler = new If(maths);
        }

        /// <summary>
        /// Executes a BOOSE program with array support.
        /// </summary>
        /// <param name="program">Program text entered by the user.</param>
        public void Run(string program)
        {
            string[] lines = program.Split('\n');

            int i = 0;

            while (i < lines.Length)
            {
                string raw = lines[i];
                string line = raw.Trim();

                if (line == "")
                {
                    i++;
                    continue;
                }

                string lower = line.ToLower();

                //for loop
                if (lower.StartsWith("for "))
                {

                    string afterFor = line.Substring(4).Trim();

                    string[] leftRight = afterFor.Split('=');
                    string varName = leftRight[0].Trim().ToLower();
                    string rightSide = leftRight[1].Trim();

                    string[] stepSplit = rightSide.Split(new string[] { "step" }, StringSplitOptions.None);
                    string rangePart = stepSplit[0].Trim();
                    string stepPart = stepSplit.Length > 1 ? stepSplit[1].Trim() : "1";

                    string[] rangeSplit = rangePart.Split(new string[] { "to" }, StringSplitOptions.None);

                    int startValue = maths.EvaluateInt(rangeSplit[0].Trim());
                    int endValue = maths.EvaluateInt(rangeSplit[1].Trim());
                    int stepValue = maths.EvaluateInt(stepPart);

                    if (!intVars.ContainsKey(varName))
                    {
                        intVars[varName] = startValue;
                    }

                    int start = i + 1;
                    int end = start;
                    int depth = 0;

                    while (end < lines.Length)
                    {
                        string check = lines[end].Trim().ToLower();

                        if (check.StartsWith("for "))
                            depth++;
                        else if (check.StartsWith("end for"))
                        {
                            if (depth == 0)
                                break;
                            depth--;
                        }

                        end++;
                    }

                    string block = "";
                    for (int k = start; k < end; k++)
                    {
                        block += lines[k] + "\n";
                    }

                    if (stepValue == 0)
                        throw new Exception("For loop step cannot be zero.");

                    if (stepValue > 0)
                    {
                        for (int loopValue = startValue; loopValue <= endValue; loopValue += stepValue)
                        {
                            intVars[varName] = loopValue;
                            Run(block);
                        }
                    }
                    else
                    {
                        for (int loopValue = startValue; loopValue >= endValue; loopValue += stepValue)
                        {
                            intVars[varName] = loopValue;
                            Run(block);
                        }
                    }

                    i = end + 1;
                    continue;
                }


                if (lower.StartsWith("end for"))
                {
                    i++;
                    continue;
                }

                // while loop
                if (lower.StartsWith("while "))
                {
                    string condition = line.Substring(6).Trim();

                    int start = i + 1;
                    int end = start;

                    while (end < lines.Length && lines[end].Trim().ToLower() != "end while")
                    {
                        end++;
                    }

                    string block = "";
                    for (int k = start; k < end; k++)
                    {
                        block += lines[k] + "\n";
                    }

                    while (ifHandler.Evaluate(condition))
                    {
                        Run(block);
                    }

                    i = end + 1;
                    continue;
                }

                
                if (lower.StartsWith("end while"))
                {
                    i++;
                    continue;
                }

                // if statement
                if (lower.StartsWith("if "))
                {
                    bool condition = ifHandler.Evaluate(line.Substring(3).Trim());

                    if (!condition)
                    {
                        int depth = 0;

                        while (i < lines.Length - 1)
                        {
                            i++;
                            string check = lines[i].Trim().ToLower();

                            if (check.StartsWith("if "))
                                depth++;

                            else if (check.StartsWith("end if"))
                            {
                                if (depth == 0) break;
                                depth--;
                            }
                            else if (check.StartsWith("else") && depth == 0)
                            {
                                break;
                            }
                        }
                    }

                    i++;
                    continue;
                }

                // else statement
                if (lower.StartsWith("else"))
                {
                    int depth = 0;

                    while (i < lines.Length - 1)
                    {
                        i++;
                        string check = lines[i].Trim().ToLower();

                        if (check.StartsWith("if "))
                            depth++;

                        else if (check.StartsWith("end if"))
                        {
                            if (depth == 0) break;
                            depth--;
                        }
                    }

                    i++;
                    continue;
                }

                // end if statement
                if (lower.StartsWith("end if"))
                {
                    i++;
                    continue;
                }

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

                    string? intName = FindIntArrayName(arrayName);
                    string? realName = FindRealArrayName(arrayName);

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
                    string[] leftRight = line.Split(('='));

                    string left = leftRight[0].Trim();
                    string right = leftRight[1].Trim();

                    string varName = left.Split(' ')[1];

                    string[] p = right.Split(' ');
                    string arrayName = p[0];
                    int index = maths.EvaluateInt(p[1]);

                    string? intName = FindIntArrayName(arrayName);
                    string? realName = FindRealArrayName(arrayName);

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
                    string[] parts = line.Substring(7)
                        .Trim()
                        .ToLower()
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    bool fill = parts.Contains("fill");

                    int r = maths.EvaluateInt(parts[0]);

                    canvas.Circle(r, fill);
                }

                // rect
                else if (lower.StartsWith("rect "))
                {
                    string args = line.Substring(5).Trim();

                    bool fill = args.ToLower().Contains("fill");

                    args = args.Replace("fill", "").Trim();

                    string[] p = args.Split(',');

                    int w = maths.EvaluateInt(p[0]);
                    int h = maths.EvaluateInt(p[1]);

                    canvas.Rect(w * 2, h * 2, fill);
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

                i++;
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

        /// <summary>Methods that AI suggested to help out with unrestricet issue</summary>
        /// <param name="name">Name of variable or array to find.</param>
        private string? FindIntArrayName(string name)
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

        private string? FindRealArrayName(string name)
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
