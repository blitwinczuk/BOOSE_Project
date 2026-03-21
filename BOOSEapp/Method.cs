using System;
using System.Collections.Generic;

namespace BOOSEapp
{
    /// <summary>
    /// Simple interpreter that supports integer methods in BOOSE.
    /// Methods do not return values explicitly – instead, the result
    /// is stored in a variable with the same name as the method.
    /// </summary>
    public class Method : IInterpreter
    {
        private AppCanvas canvas;

        // Stores all integer variables including method results
        private Dictionary<string, int> intVars = new Dictionary<string, int>();

        // Stores method name -> method body as text
        private Dictionary<string, string> methods = new Dictionary<string, string>();

        // Stores method name -> parameter names
        private Dictionary<string, string[]> parameters = new Dictionary<string, string[]>();

        private Maths maths;

        /// <summary>
        /// Constructor - initialises canvas and maths engine.
        /// </summary>
        public Method(AppCanvas canvas)
        {
            this.canvas = canvas;
            maths = new Maths(intVars, null);
        }

        /// <summary>
        /// Runs the BOOSE program line by line.
        /// </summary>
        public void Run(string program)
        {
            string[] lines = program.Split('\n');
            int i = 0;

            while (i < lines.Length)
            {
                string line = lines[i].Trim()
                    .Replace("\uFEFF", "")
                    .Replace("\u200B", "");

                // Skip empty lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    i++;
                    continue;
                }

                string lower = line.ToLower();

                // method declaration - defines a new method with given name and parameters
                if (lower.StartsWith("method int "))
                {
                    // Extract method header
                    string header = line.Substring(11).Trim();

                    // Split into method name and parameters
                    string[] parts = header.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                    string methodName = parts[0].ToLower();

                    string paramPart = parts.Length > 1 ? parts[1] : "";
                    paramPart = paramPart.Replace("int ", "");

                    string[] paramList = paramPart.Split(',', StringSplitOptions.RemoveEmptyEntries);

                    // Clean parameter names
                    for (int p = 0; p < paramList.Length; p++)
                    {
                        paramList[p] = paramList[p].Trim().ToLower();
                    }

                    // Find method body until end method
                    int start = i + 1;
                    int end = start;

                    while (end < lines.Length && lines[end].Trim().ToLower() != "end method")
                    {
                        end++;
                    }

                    if (end >= lines.Length)
                    {
                        throw new Exception("Missing end method for " + methodName);
                    }

                    // Store method body as a block of text
                    string body = string.Join("\n", lines[start..end]);

                    methods[methodName] = body;
                    parameters[methodName] = paramList;

                    // Create variable to store result of method
                    if (!intVars.ContainsKey(methodName))
                    {
                        intVars[methodName] = 0;
                    }

                    // Skip to line after "end method"
                    i = end + 1;
                    continue;
                }

                // Skip closing keyword
                if (lower == "end method")
                {
                    i++;
                    continue;
                }

                // call method - executes a method with given arguments
                if (lower.StartsWith("call "))
                {
                    string[] parts = line.Substring(5).Trim()
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    string methodName = parts[0].ToLower();

                    // Check if method exists
                    if (!methods.ContainsKey(methodName))
                    {
                        throw new Exception("Method not found: " + methodName);
                    }

                    string[] paramList = parameters[methodName];

                    // Check correct number of arguments
                    if (parts.Length - 1 != paramList.Length)
                    {
                        throw new Exception("Wrong number of parameters for method: " + methodName);
                    }

                    // Backup current values to simulate local scope
                    Dictionary<string, int> backup = new Dictionary<string, int>();

                    foreach (string param in paramList)
                    {
                        if (intVars.ContainsKey(param))
                        {
                            backup[param] = intVars[param];
                        }
                    }

                    // Assign argument values to parameters
                    for (int p = 0; p < paramList.Length; p++)
                    {
                        intVars[paramList[p]] = maths.EvaluateInt(parts[p + 1]);
                    }

                    // Execute method body
                    ExecuteBlock(methods[methodName]);

                    // Restore previous values
                    foreach (string param in paramList)
                    {
                        if (backup.ContainsKey(param))
                        {
                            intVars[param] = backup[param];
                        }
                        else
                        {
                            intVars.Remove(param);
                        }
                    }

                    i++;
                    continue;
                }

                // Execute normal command
                ExecuteLine(line);
                i++;
            }
        }

        /// <summary>
        /// Executes multiple lines of code (used for method body).
        /// </summary>
        private void ExecuteBlock(string block)
        {
            string[] lines = block.Split('\n');

            foreach (string raw in lines)
            {
                string line = raw.Trim();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    ExecuteLine(line);
                }
            }
        }

        /// <summary>
        /// Executes a single BOOSE command.
        /// </summary>
        private void ExecuteLine(string line)
        {
            string lower = line.ToLower();

            // variable declaration - creates new variable with optional initial value
            if (lower.StartsWith("int "))
            {
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string name = parts[1].ToLower();

                if (line.Contains("="))
                {
                    string value = line.Split('=')[1].Trim();
                    intVars[name] = maths.EvaluateInt(value);
                }
                else
                {
                    intVars[name] = 0;
                }
            }

            // assignment - updates value of existing variable or creates new one if it doesn't exist
            else if (line.Contains("="))
            {
                string[] parts = line.Split('=');
                string name = parts[0].Trim().ToLower();
                string value = parts[1].Trim();

                intVars[name] = maths.EvaluateInt(value);
            }

            // All other commands are related to drawing on the canvas
            else if (lower.StartsWith("moveto "))
            {
                string[] parts = line.Substring(7).Split(',');
                int x = maths.EvaluateInt(parts[0]);
                int y = maths.EvaluateInt(parts[1]);
                canvas.MoveTo(x, y);
            }
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
            else if (lower.StartsWith("rect "))
            {
                string args = line.Substring(5).Trim();

                bool fill = args.ToLower().Contains("fill");

                args = args.Replace("fill", "").Trim();

                string[] p = args.Split(',');

                int w = maths.EvaluateInt(p[0]);
                int h = maths.EvaluateInt(p[1]);

                canvas.Rect(w, h, fill);
            }
            else if (lower.StartsWith("pen "))
            {
                string[] parts = line.Substring(4).Split(',');
                int r = maths.EvaluateInt(parts[0]);
                int g = maths.EvaluateInt(parts[1]);
                int b = maths.EvaluateInt(parts[2]);
                canvas.SetColour(r, g, b);
            }

            // write command - writes either the value of an integer variable or a string literal
            else if (lower.StartsWith("write "))
            {
                string name = line.Substring(6).Trim().ToLower();

                if (intVars.ContainsKey(name))
                {
                    canvas.WriteText(intVars[name].ToString());
                }
                else
                {
                    canvas.WriteText(name);
                }
            }
        }
    }
}