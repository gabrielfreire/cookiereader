using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CookieReader
{
    [HelpOption("--help")]
    public class CookieReaderCmdBase
    {
        protected IConsole _console;

        protected virtual Task<int> OnExecute(CommandLineApplication app)
        {
            return Task.FromResult(0);
        }
        protected void OutputToFile(string outputFile, string data)
        {
            File.WriteAllText(outputFile, data);
        }

        protected void OutputToConsole(string data, ConsoleColor color = ConsoleColor.White)
        {
            _console.BackgroundColor = ConsoleColor.Black;
            _console.ForegroundColor = color;
            _console.Out.Write(data);
            _console.ResetColor();
        }

        protected void OutputError(string message)
        {
            _console.BackgroundColor = ConsoleColor.Black;
            _console.ForegroundColor = ConsoleColor.Red;
            _console.Error.WriteLine(message);
            _console.ResetColor();
        }
    }
}
