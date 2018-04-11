using System;
using SynopticS.ConsoleFormat;

namespace SynopticS.Exceptions
{
    public abstract class CommandParseExceptionBase : Exception
    {
        public abstract void Render();
        
        public ConsoleFormatter ConsoleFormatter
        {
            get
            {
                return new ConsoleFormatter(ConsoleWriter.Error);    
            }
        }
    }
}