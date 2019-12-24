using Autofac.Extensions.DependencyInjection;
using BookService.CommonEntities.Pathes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BookService.WebApi
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(x => x.UseStartup<Startup>()
                                                       .UseKestrel()
                                                       .UseContentRoot(DirectoryPath.Current))
                       .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                       .UseSerilog();
        }
    }
}
