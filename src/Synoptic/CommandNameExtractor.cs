﻿using System.Collections.Generic;
using System.Linq;

namespace ConsoleWrapper.Synoptic
{
    public static class CommandNameExtractor
    {
        public static string Extract(IEnumerable<CommandLineParseResult> parsedCommandLineResults)
        {
            return parsedCommandLineResults.FirstOrDefault(p => 
                p.AdditionalParameters != null && p.AdditionalParameters.Length > 0)
                .AdditionalParameters.FirstOrDefault();
        }
    }
}