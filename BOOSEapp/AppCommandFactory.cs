using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOSEapp
{
    internal class AppCommandFactory : CommandFactory
    {
        public override ICommand MakeCommand(string commandType)
        {
            if (commandType == "circle")
                return new Circle();
            else if (commandType == "rectangle")
                return new Rectangle();
            else if (commandType == "moveto")
                return new MoveTo();
            else
                return base.MakeCommand(commandType);
        }

        ICanvas canvas;

        public AppCommandFactory(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        
    }
}
