using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.IO;

namespace PokeApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
