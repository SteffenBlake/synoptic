﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mono.Options;
using Synoptic.ConsoleUtilities;

namespace Synoptic.HelpUtilities
{
    internal class CommandLineHelpGenerator
    {
        public static CommandLineHelp Generate(IEnumerable<CommandAction> commandActions)
        {
            return new CommandLineHelp(commandActions);
        }

        private string GetUsagePattern(IEnumerable<Option> optionSet)
        {
            var output = new StringBuilder();

            // Generate usage.
            if (optionSet != null)
            {
                foreach (Option o in optionSet)
                {
                    if (o.OptionValueType == OptionValueType.None)
                    {
                        output.AppendFormat("[--{0}]", o.Prototype);
                    }
                    if (o.OptionValueType == OptionValueType.Required)
                    {
                        output.AppendFormat("[--{0}VALUE]", o.Prototype);
                    }

                    output.Append(" ");
                }
            }

            return output.Append("COMMAND ACTION [ARGS]").ToString();
        }

        public void ShowCommandHelp(IEnumerable<Command> availableCommands, OptionSet optionSet)
        {
            var processName = Process.GetCurrentProcess().ProcessName;
            var usagePreamble = String.Format("Usage: {0} ", processName);
            ConsoleFormatter.Write(usagePreamble);

            var usagePattern = GetUsagePattern(optionSet);
            ConsoleFormatter.Write(usagePreamble.Length, usagePattern);
            Console.WriteLine("\n");

            const int spacingWidth = 3;

            var spacer = new string(' ', spacingWidth);
            var maximumCommandNameLength = availableCommands.Count() > 0 ? availableCommands.Max(c => c.Name.Length) : 0;

            ConsoleFormatter.Write("The available commands are:\n");

            foreach (var command in availableCommands)
            {
                ConsoleFormatter.Write(spacer.Length, String.Format("{0," + -maximumCommandNameLength + "}{1}", command.Name, spacer));
                ConsoleFormatter.Write(Console.CursorLeft, String.Format("{0}\n", command.Description));
            }
        }
    }
}