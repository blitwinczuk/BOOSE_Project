namespace BOOSEapp
{
    /// <summary>
    /// Singleton logger used to write debug information.
    /// </summary>
    public sealed class LogInDebug
    {
        private static LogInDebug instance;

        /// <summary>
        /// Gets the single logger instance.
        /// </summary>
        public static LogInDebug Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LogInDebug();
                }

                return instance;
            }
        }

        private LogInDebug()
        {
        }

        /// <summary>
        /// Writes a message to the debug output.
        /// </summary>
        public void Log(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
