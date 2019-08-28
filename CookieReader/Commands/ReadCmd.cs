using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CookieReader.Commands
{
    [Command(Name ="read", Description ="Read a Google Chrome cookie file")]
    public class ReadCmd : CookieReaderCmdBase
    {
        [Option(CommandOptionType.SingleValue, ShortName = "f", LongName = "filter-text", Description = "filer result by some text", ValueName = "string filter", ShowInHelpText = true)]
        public string StringFilter { get; set; }
        [Option(CommandOptionType.SingleValue, ShortName = "o", LongName = "output-file", Description = "output file", ValueName = "output file", ShowInHelpText = true)]
        public string OutputFile { get; set; }

        public ReadCmd(IConsole console)
        {
            _console = console;
        }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {

            //OutputToConsole($"Filter by   [{StringFilter}]\r\n");
            //OutputToConsole($"Output File [{OutputFile}]\r\n");
            try
            {
                var cookies = ReadCookies();
                var val     = "";

                foreach (var cookie in cookies)
                {
                    if (string.IsNullOrEmpty(StringFilter))
                    {
                        val += $"Cookie-Name: {cookie.Item1}, Cookie-Value: {cookie.Item2}\r\n\r\n";

                    }
                    else
                    {
                        if (cookie.Item1.IndexOf(StringFilter) > -1 || cookie.Item2.IndexOf(StringFilter) > -1)
                        {
                            val += $"Cookie-Name: {cookie.Item1}, Cookie-Value: {cookie.Item2}\r\n\r\n";
                        }
                    }
                }

                var willOutputToFile = await ShouldOutputToFile();
                if (willOutputToFile)
                {
                    OutputToConsole($"Saving to {OutputFile}\r\n", color: ConsoleColor.Blue);
                    OutputToFile(OutputFile, val);
                }
                else
                {
                    OutputToConsole(val, color: ConsoleColor.Cyan);
                }

                return 0;
            }
            catch (Exception ex)
            {
                OutputError(ex.Message);
                return 1;
            }
        }

        private IEnumerable<Tuple<string, string>> ReadCookies()
        {
            var dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Cookies";
            OutputToConsole($"Cookies File found at {dbPath}\r\n");
            if (File.Exists(dbPath))
            {
                var connectionString = "Data Source=" + dbPath + ";pooling=false";
                using (var conn = new System.Data.SQLite.SQLiteConnection(connectionString))
                {

                    OutputToConsole($"Connecting...\r\n");
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT name, encrypted_value FROM cookies";

                        OutputToConsole($"Executing command...\r\n");
                        using (var reader = cmd.ExecuteReader())
                        {
                            OutputToConsole($"Processing... please wait!\r\n", color: ConsoleColor.Green);
                            while (reader.Read())
                            {
                                var encryptedData = (byte[])reader[1];
                                var decodedData   = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                                var plainText     = Encoding.ASCII.GetString(decodedData);
                                yield return Tuple.Create(reader.GetString(0), plainText);
                            }
                        }
                    }
                }
            }
            else
            {
                OutputError($"No Cookie file was found at {dbPath}");
            }
        }

        private Task<bool> ShouldOutputToFile()
        {
            if (string.IsNullOrEmpty(OutputFile))
            {
                return Task.FromResult(false);
            }

            if (File.Exists(OutputFile))
            {
                File.Delete(OutputFile);
                File.Create(OutputFile).Close();
            }
            else
            {
                File.Create(OutputFile).Close();
            }

            return Task.FromResult(true);
        }
    }
}
