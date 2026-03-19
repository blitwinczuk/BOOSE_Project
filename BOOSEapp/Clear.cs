using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// Clears the drawing canvas.
    /// </summary>
    internal class Clear : CommandOneParameter

    {
        /// <summary>
        /// Initializes a new instance of the Clear command.
        /// </summary>
        public Clear() : base()
        {
        }

        /// <summary>
        /// Executes the clear command and clears the canvas.
        /// </summary>
        public override void Execute()
        {
            base.Execute();
            Canvas.Clear();
        }
    }
}
