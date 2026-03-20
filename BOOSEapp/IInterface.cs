namespace BOOSEapp
{
    /// <summary>
    /// Common interface for classes that can run a BOOSE program.
    /// </summary>
    public interface IInterpreter
    {
        /// <summary>
        /// Runs the supplied BOOSE program.
        /// </summary>
        /// <param name="program">Program entered by the user.</param>
        void Run(string program);
    }
}
