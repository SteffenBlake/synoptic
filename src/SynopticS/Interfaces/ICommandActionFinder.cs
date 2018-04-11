using System.Collections.Generic;

namespace SynopticS
{
    internal interface ICommandActionFinder
    {
        IEnumerable<CommandAction> FindInCommand(Command command);
    }
}