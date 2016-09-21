using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace PokeApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        await context.Response.WriteAsync("This will be awesome");
                    });
                })
                .Build();

            host.Run();
        }
    }
}
