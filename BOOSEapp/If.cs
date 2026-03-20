namespace BOOSEapp
{
    /// <summary>
    /// Handles IF, ELSE and END IF logic in BOOSE programs.
    /// </summary>
    public class If
    {
        private Maths maths;

        public If(Maths maths)
        {
            this.maths = maths;
        }

        /// <summary>
        /// Evaluates conditions like: <c>x &lt; 10</c>, <c>x &gt; 5</c>, <c>x == 3</c>.
        /// </summary>
        public bool Evaluate(string cond)
        {
            cond = cond.Trim();

            if (cond.Contains(">"))
            {
                string[] p = cond.Split('>');
                return maths.EvaluateInt(p[0]) > maths.EvaluateInt(p[1]);
            }
            else if (cond.Contains("<"))
            {
                string[] p = cond.Split('<');
                return maths.EvaluateInt(p[0]) < maths.EvaluateInt(p[1]);
            }
            else if (cond.Contains("=="))
            {
                string[] p = cond.Split(new string[] { "==" }, StringSplitOptions.None);
                return maths.EvaluateInt(p[0]) == maths.EvaluateInt(p[1]);
            }

            return false;
        }
    }
}
