using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// Custom command factory used to create BOOSE commands.
    /// It extends the default command factory and returns
    /// application-specific command implementations.
    /// </summary>
    public class AppCommandFactory : CommandFactory
    {
        private ICanvas canvas;

        /// <summary>
        /// Initializes a new instance of the AppCommandFactory class.
        /// </summary>
        /// <param name="canvas">Canvas used by commands created by the factory.</param>
        public AppCommandFactory(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        /// <summary>
        /// Creates a command object based on the command name.
        /// </summary>
        /// <param name="commandType">The name of the BOOSE command.</param>
        /// <returns>An <see cref="ICommand"/> implementation for the requested command.</returns>
        public override ICommand MakeCommand(string commandType)
        {
            if (commandType == "circle")
                return new Circle();
            else if (commandType == "rect")
                return new Rectangle();
            else if (commandType == "moveto")
                return new MoveTo();
            else if (commandType == "clear")
                return new Clear();
            else
                return base.MakeCommand(commandType);
        }
    }
}
