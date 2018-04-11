using SynopticS.ConsoleFormat;

namespace SynopticS.Exceptions
{
    public class NoCommandsDefinedException : CommandParseExceptionBase
    {
        public override void Render()
        {
            ConsoleFormatter.Write(new ConsoleTable()
               .AddRow(
                   new ConsoleCell("There are currently no commands defined.").WithPadding(0))
                   .AddRow(
                   new ConsoleCell("Please ensure commands are correctly defined and registered within Synoptic using the [Command] attribute.")));

        }
    }
}