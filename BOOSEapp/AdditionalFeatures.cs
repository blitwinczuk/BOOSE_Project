using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace BOOSEapp
{
    /// <summary>
    /// Provides additional BOOSE features such as random values,
    /// user input, and named colours.
    /// </summary>
    public class AdditionalFeatures
    {
        private AppCanvas canvas;
        private Dictionary<string, int> intVars;
        private Random rnd = new Random();

        /// <summary>
        /// Initialises the additional features helper.
        /// </summary>
        /// <param name="canvas">Canvas used for drawing commands.</param>
        /// <param name="intVars">Integer variables used by the interpreter.</param>
        public AdditionalFeatures(AppCanvas canvas, Dictionary<string, int> intVars)
        {
            this.canvas = canvas;
            this.intVars = intVars;
        }

        /// <summary>
        /// Tries to execute an additional feature command.
        /// </summary>
        /// <param name="line">Single line of BOOSE code.</param>
        /// <returns>True if the command was handled; otherwise false.</returns>
        public bool TryExecute(string line)
        {
            string lower = line.ToLower().Trim();

            // random x 1 100
            if (lower.StartsWith("random "))
            {
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 4)
                    throw new Exception("Random command requires: random name min max");

                string name = parts[1].ToLower();
                int min = int.Parse(parts[2]);
                int max = int.Parse(parts[3]);

                intVars[name] = rnd.Next(min, max + 1);
                return true;
            }

            // input x
            if (lower.StartsWith("input "))
            {
                string name = line.Substring(6).Trim().ToLower();

                string value = Interaction.InputBox(
                    "Enter a value for " + name,
                    "BOOSE Input",
                    "0");

                if (!int.TryParse(value, out int result))
                    throw new Exception("Input value must be an integer.");

                intVars[name] = result;
                return true;
            }

            // colour
            if (lower.StartsWith("colour "))
            {
                string colourName = line.Substring(7).Trim().ToLower();

                if (colourName == "red")
                    canvas.SetColour(255, 0, 0);
                else if (colourName == "green")
                    canvas.SetColour(0, 255, 0);
                else if (colourName == "blue")
                    canvas.SetColour(0, 0, 255);
                else if (colourName == "black")
                    canvas.SetColour(0, 0, 0);
                else
                    throw new Exception("Unknown colour name: " + colourName);

                return true;
            }

            return false;
        }
    }
}
