using CookieReader.Commands;
using McMaster.Extensions.CommandLineUtils;
using System.Reflection;
using System.Threading.Tasks;

namespace CookieReader
{
    [Command(Name = "cookiereader", ThrowOnUnexpectedArgument = false, OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    //[VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(typeof(ReadCmd))]
    public class CookieReaderCmd : CookieReaderCmdBase
    {
        public CookieReaderCmd(IConsole console)
        {
            _console = console;
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult(0);
        }

        private static object GetVersion()
            => typeof(CookieReaderCmd).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
