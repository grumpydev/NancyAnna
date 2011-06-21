using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyAnna
{
    class Program
    {
        static void Main(string[] args)
        {
            var bootstrapper = Nancy.Bootstrapper.NancyBootstrapperLocator.Bootstrapper;

            bootstrapper.Initialise();
            var engine = bootstrapper.GetEngine();

            using (var server = new Anna.HttpServer("http://*:4789/"))
            {
                Console.WriteLine("Listening on http://localhost:4789/");

                server.RAW("/*").Subscribe(ctx =>
                    {
                        var request = ctx.Request.ToNancyRequest();

                        var result = engine.HandleRequest(request);

                        ctx.Respond(new NancyResponse(result.Response));
                    });

                Console.WriteLine("Press [ENTER] to stop.");
                Console.ReadLine();
            }
        }
    }
}
