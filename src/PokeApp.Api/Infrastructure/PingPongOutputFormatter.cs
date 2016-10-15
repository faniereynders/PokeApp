using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.Api.Infrastructure
{
    public class PingPongOutputFormatter : IOutputFormatter
    {
        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            var httpContext = ((OutputFormatterWriteContext)context).HttpContext;
            var allowedContentTypes = new string[] { "text/ascii", "image/png" };

            return 
                httpContext.Request.Path == "/ping" && 
                allowedContentTypes.Contains(httpContext.Request.ContentType);
        }

        public async Task WriteAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            switch (context.HttpContext.Request.ContentType)
            {
                case "text/ascii":
                    await renderText(context.HttpContext.Response);
                    break;
                case "image/png":
                    await renderImage(context.HttpContext.Response);
                    break;
                default:
                    break;
            }
        }

        private async Task renderImage(HttpResponse response)
        {
            response.ContentType = "image/png";
            var b = File.ReadAllBytes(@".\Content\pong.png");

            await response.Body.WriteAsync(b, 0, b.Length);
        }

        private async Task renderText(HttpResponse response)
        {
            const string output = @"
PPPPPPPPPPPPPPPPP        OOOOOOOOO     NNNNNNNN        NNNNNNNN        GGGGGGGGGGGGG !!! 
P::::::::::::::::P     OO:::::::::OO   N:::::::N       N::::::N     GGG::::::::::::G!!:!!
P::::::PPPPPP:::::P  OO:::::::::::::OO N::::::::N      N::::::N   GG:::::::::::::::G!:::!
PP:::::P     P:::::PO:::::::OOO:::::::ON:::::::::N     N::::::N  G:::::GGGGGGGG::::G!:::!
  P::::P     P:::::PO::::::O   O::::::ON::::::::::N    N::::::N G:::::G       GGGGGG!:::!
  P::::P     P:::::PO:::::O     O:::::ON:::::::::::N   N::::::NG:::::G              !:::!
  P::::PPPPPP:::::P O:::::O     O:::::ON:::::::N::::N  N::::::NG:::::G              !:::!
  P:::::::::::::PP  O:::::O     O:::::ON::::::N N::::N N::::::NG:::::G    GGGGGGGGGG!:::!
  P::::PPPPPPPPP    O:::::O     O:::::ON::::::N  N::::N:::::::NG:::::G    G::::::::G!:::!
  P::::P            O:::::O     O:::::ON::::::N   N:::::::::::NG:::::G    GGGGG::::G!:::!
  P::::P            O:::::O     O:::::ON::::::N    N::::::::::NG:::::G        G::::G!!:!!
  P::::P            O::::::O   O::::::ON::::::N     N:::::::::N G:::::G       G::::G !!! 
PP::::::PP          O:::::::OOO:::::::ON::::::N      N::::::::N  G:::::GGGGGGGG::::G     
P::::::::P           OO:::::::::::::OO N::::::N       N:::::::N   GG:::::::::::::::G !!! 
P::::::::P             OO:::::::::OO   N::::::N        N::::::N     GGG::::::GGG:::G!!:!!
PPPPPPPPPP               OOOOOOOOO     NNNNNNNN         NNNNNNN        GGGGGG   GGGG !!! 
";
            var b = Encoding.UTF8.GetBytes(output);
            await response.Body.WriteAsync(b, 0, b.Length);
        }
    }
}
