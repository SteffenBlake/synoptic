namespace SynopticS
{
    internal interface ICommandLineParser
    {
        CommandLineParseResult Parse(CommandAction action,  string[] args);
    }
}