﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Synoptic.HelpUtilities;

namespace Synoptic
{
    public class CommandRunner
    {
        private readonly TextWriter _error = Console.Error;

        private readonly CommandActionManifest _actionManifest = new CommandActionManifest();
        private readonly ICommandActionFinder _actionFinder = new CommandActionActionFinder();
        private IDependencyResolver _resolver = new ActivatorDependencyResolver();
        private CommandLineHelp _help;
        private Func<CommandLineParseResult, object> _commandSetInstantiator;
        private Func<string[], string[]> _preProcessor;

        public CommandRunner WithDependencyResolver(IDependencyResolver resolver)
        {
            _resolver = resolver;
            return this;
        }

        public CommandRunner WithCommandsFromType<T>()
        {
            _actionManifest.Commands.AddRange(_actionFinder.FindInType(typeof(T)).Commands);
            return this;
        }

        public CommandRunner WithCommandsFromAssembly(Assembly assembly)
        {
            _actionManifest.Commands.AddRange(_actionFinder.FindInAssembly(assembly).Commands);
            return this;
        }

        public void Run(string[] args)
        {
            if (_actionManifest.Commands.Count == 0)
                WithCommandsFromAssembly(Assembly.GetCallingAssembly());

            if (_actionManifest.Commands.Count == 0)
            {
                _error.WriteLine("There are currently no commands defined.\nPlease ensure commands are correctly defined and registered within Synoptic.");
                return;
            }

            if (_help == null)
                _help = CommandLineHelpGenerator.Generate(_actionManifest);

            if (args == null || args.Length == 0)
            {
                ShowHelp();
                return;
            }

            try
            {
                ICommandLineParser parser = new CommandLineParser();
                
                CommandLineParseResult parseResult = parser.Parse(_actionManifest, args, _preProcessor);
                if (!parseResult.WasSuccessfullyParsed)
                    throw new CommandActionException(parseResult.Message);

                if (_commandSetInstantiator != null)
                    parseResult.CommandAction.Run(_commandSetInstantiator(parseResult), parseResult);
                else
                    parseResult.CommandAction.Run(_resolver, parseResult);
            }
            catch (CommandActionException commandException)
            {
                ShowErrorMessage(commandException);
                ShowHelp();
            }
            catch (TargetInvocationException targetInvocationException)
            {
                Exception innerException = targetInvocationException.InnerException;

                if (innerException == null) throw;

                if (innerException is CommandActionException)
                {
                    ShowErrorMessage(innerException);
                    ShowHelp();
                }

                throw new CommandInvocationException("Error executing command", innerException);
            }
        }

        private void ShowErrorMessage(Exception exception)
        {
            _error.WriteLine(exception.Message);
        }

        private void ShowHelp()
        {
            _error.WriteLine();
            _error.WriteLine("Usage: {0} <command> [options]", Process.GetCurrentProcess().ProcessName);
            _error.WriteLine();

            foreach (var command in _help.Commands)
            {
                _error.WriteLine(command.FormattedLine);
                foreach (var parameter in command.Parameters)
                {
                    _error.WriteLine(parameter.FormattedLine);
                }

                _error.WriteLine();
            }
        }

        public CommandRunner WithCommandSet<T>(Func<CommandLineParseResult, object> commandSetInstantiator)
        {
            _actionManifest.Commands.AddRange(_actionFinder.FindInType(typeof(T)).Commands);
            _commandSetInstantiator = commandSetInstantiator;

            return this;
        }

        public CommandRunner WithArgsPreProcessor(Func<string[], string[]> preProcessor)
        {
            _preProcessor = preProcessor;
            return this;
        }
    }
}