using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EmptyService.DependencyResolver;
using EmptyService.Logger.Abstractions;
using EmptyService.WebApi.Helpers;
using Job;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmptyService.WebApi
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                            .AddConfiguration(configuration)
                            .AddJsonFile("appsettings.json")
                            .AddEnvironmentVariables()
                            .Build();

            shouldStartJob = bool.Parse(Configuration.GetSection("JobSettings:ShouldStart").Value);
        }

        private const string CorsPolicyName = "DefaultCorsPolicy";

        private static void SubscribeLogException()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Console.WriteLine(e);

                throw (Exception)e.ExceptionObject;
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Console.WriteLine(e);
                e.SetObserved();

                throw e.Exception;
            };
        }

        private IConfiguration Configuration { get; }

        private readonly bool shouldStartJob;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SubscribeLogException();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var logger = app.ApplicationServices.GetAutofacRoot().Resolve<ILog>();
            Resolver.Validate(app.ApplicationServices.GetAutofacRoot(), logger);
            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
            logger.Information($"Listening on {string.Join(", ", serverAddressesFeature.Addresses)}");

            app.UseCors(CorsPolicyName);
            app.UseHttpsRedirection();
            app.UseRouting();

            // app.UseHealthChecks("/health");
            app.UseAuthorization();
            app.UseEndpoints(x => x.MapControllers());
        }

        // This method gets called by the runtime.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            Resolver.Register(builder);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .ConfigureApplicationPartManager(x =>
                                                         x.FeatureProviders
                                                          .Add(new InternalControllerFeatureProvider()));

            if (shouldStartJob)
            {
                services.AddHostedService<JobInstance>();
            }

            services.AddCors(x => x.AddPolicy(CorsPolicyName,
                                              y => y
                                                   .AllowAnyMethod()
                                                   .AllowAnyOrigin()
                                                   .AllowAnyHeader()));
        }
    }
}
