namespace ConsoleApp.IO.Consoles;

public interface ICommandLine
{
    CommandLineCommand CommandLineArgumentParser(string[] args);

    string[] GetArguments();
}
