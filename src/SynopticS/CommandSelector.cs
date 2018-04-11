using System.Collections.Generic;
using SynopticS.Exceptions;

namespace SynopticS
{
    internal class CommandSelector
    {
        public Command Select(string commandName, List<Command> availableCommands)
        {
            var command = new MatchSelector<Command>().Match(commandName, availableCommands, c => c.Name);
            if (command != null)
                return command;

            var exception = new CommandNotFoundException(commandName);
            exception.AvailableCommands.AddRange(availableCommands);

            var possibleCommands = new MatchSelector<Command>().PartialMatch(commandName, availableCommands, c => c.Name);
            exception.PossibleCommands.AddRange(possibleCommands);

            throw exception;
        }
    }
}