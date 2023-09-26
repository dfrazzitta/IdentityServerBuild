using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MvcClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    /*
                     * FOR /f "tokens=*" %i IN ('docker ps -q') DO docker stop %i
                     * tcpdump --interface any -vv -w capX.txt
                     * tcpdump -D
                     * from windows -- docker cp container:/app c:\path
                     */
                    webBuilder.ConfigureKestrel(options =>
                    {
                        var port = 443;
                        var pfxFilePath = @"/app/mvcclient.pfx";
                        // The password you specified when exporting the PFX file using OpenSSL.
                        // This would normally be stored in configuration or an environment variable;
                        // I've hard-coded it here just to make it easier to see what's going on.
                        var pfxPassword = "";

                        options.Listen(IPAddress.Any, port, listenOptions =>
                        {
                            // Enable support for HTTP1 and HTTP2 (required if you want to host gRPC endpoints)
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                            // Configure Kestrel to use a certificate from a local .PFX file for hosting HTTPS
                            listenOptions.UseHttps(pfxFilePath, pfxPassword);
                        });
                    });
                     
                    webBuilder.UseStartup<Startup>();
                });
    }
}
