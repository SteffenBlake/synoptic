using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SynopticS.Exceptions;

namespace SynopticS
{
    public class CommandRunner : ICommandRunner
    {
        private readonly HelpGenerator _helpGenerator = new HelpGenerator();
        private IServiceProvider ServiceProvider { get; }

        public CommandRunner(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Run(string[] args)
        {
            if (args == null)
                args = new string[0];

            var arguments = new Queue<string>(args);

            try
            {
                if (arguments.Count == 0)
                {
                    _helpGenerator.ShowCommandUsage(_availableCommands, _optionSet);
                    return;
                }

                var commandName = arguments.Dequeue();

                var commandSelector = new CommandSelector();
                var command = commandSelector.Select(commandName, _availableCommands);

                var actionName = arguments.Count > 0 ? arguments.Dequeue() : null;
                var availableActions = _commandActionFinder.FindInCommand(command);

                if (actionName == null)
                {
                    _helpGenerator.ShowCommandHelp(command, availableActions.ToList());
                    return;
                }

                var actionSelector = new ActionSelector();
                var action = actionSelector.Select(actionName, command, availableActions);

                var parser = new CommandLineParser();

                var parseResult = parser.Parse(action, arguments.ToArray());
                parseResult.CommandAction.Run(ServiceProvider, parseResult);
            }
            catch (CommandParseExceptionBase exception)
            {
                exception.Render();
            }
            catch (TargetInvocationException exception)
            {
                var innerException = exception.InnerException;
                if (innerException == null)
                    throw;

                if (innerException is CommandParseExceptionBase)
                {
                    ((CommandParseExceptionBase)exception.InnerException).Render();
                    return;
                }

                throw new CommandInvocationException("Error executing command", innerException);
            }
        }
    }
}