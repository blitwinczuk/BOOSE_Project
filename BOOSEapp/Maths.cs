using System;
using System.Collections.Generic;
using System.Globalization;

namespace BOOSEapp
{
    /// <summary>
    /// Provides simple mathematical expression evaluation (+, -, *, /)
    /// </summary>
    public class Maths
    {
        private Dictionary<string, int> intVars;
        private Dictionary<string, double> realVars;

        /// <summary>
        /// Initializes Maths helper with variable dictionaries.
        /// </summary>
        /// <param name="intVars">Integer variables.</param>
        /// <param name="realVars">Real variables.</param>
        public Maths(Dictionary<string, int> intVars,
                     Dictionary<string, double> realVars)
        {
            this.intVars = intVars;
            this.realVars = realVars;
        }

        /// <summary>
        /// Evaluates expression and returns integer result.
        /// </summary>
        public int EvaluateInt(string expr)
        {
            return (int)Evaluate(expr);
        }

        /// <summary>
        /// Evaluates expression and returns real result.
        /// </summary>
        public double EvaluateReal(string expr)
        {
            return Evaluate(expr);
        }

        /// <summary>
        /// Evaluates simple expression (+, -, *, /).
        /// </summary>
        private double Evaluate(string expr)
        {
            expr = expr.Trim();

            // negative number, e.g. -2 or -10.5
            if (double.TryParse(expr, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                return number;
            }

            if (expr.Contains("+"))
            {
                var p = expr.Split('+');
                return GetValue(p[0]) + GetValue(p[1]);
            }
            else if (expr.Contains("*"))
            {
                var p = expr.Split('*');
                return GetValue(p[0]) * GetValue(p[1]);
            }
            else if (expr.Contains("/"))
            {
                var p = expr.Split('/');
                return GetValue(p[0]) / GetValue(p[1]);
            }
            else if (expr.Contains("-"))
            {
                int minusIndex = expr.IndexOf('-');

                // subtraction only if '-' is not the first character
                if (minusIndex > 0)
                {
                    string left = expr.Substring(0, minusIndex);
                    string right = expr.Substring(minusIndex + 1);
                    return GetValue(left) - GetValue(right);
                }
            }

            return GetValue(expr);
        }

        /// <summary>
        /// Gets value from variables or parses number.
        /// </summary>
        private double GetValue(string val)
        {
            val = val.Trim();

            if (intVars != null && intVars.ContainsKey(val))
                return intVars[val];

            if (realVars != null && realVars.ContainsKey(val))
                return realVars[val];

            return double.Parse(val, CultureInfo.InvariantCulture);
        }
    }
}