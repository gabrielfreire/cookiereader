using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace CookieReader
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                });
            try
            {
                return await builder.RunCommandLineApplicationAsync<CookieReaderCmd>(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}
