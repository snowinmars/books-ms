using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Core;
using EmptyService.CommonEntities;
using EmptyService.CommonEntities.Exceptions;
using EmptyService.CommonEntities.Pathes;
using EmptyService.Configuration;
using EmptyService.Configuration.Abstractions;
using EmptyService.DependencyResolver.ConfigurationModels;
using EmptyService.Logger;
using EmptyService.Logger.Abstractions;
using Logic;
using Logic.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using Repository;
using Repository.Abstractions;

namespace EmptyService.DependencyResolver
{
    // ReSharper disable once AllowPublicClass
    public static class Resolver
    {
        private static readonly string ConfigFileName = "connection-strings.json";

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static void Register()
        {
            var builder = new ContainerBuilder();

            Register(builder);
        }

        public static void Register(ContainerBuilder builder)
        {
            var (config, logger) = Init();

            logger.Information("Resolving dependencies");

            BuildContainer(builder, logger, config);
        }

        public static void RegisterContext<TContext>(ContainerBuilder builder, string connectionString)
            where TContext : DbContext
        {
            builder.Register(componentContext =>
                   {
                       var serviceProvider = componentContext.Resolve<IServiceProvider>();
                       var dbContextOptions = new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>());
                       var optionsBuilder = new DbContextOptionsBuilder<TContext>(dbContextOptions)
                                            .UseApplicationServiceProvider(serviceProvider)
                                            .UseNpgsql(connectionString,
                                                          serverOptions => serverOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null));

                       return optionsBuilder.Options;
                   }).As<DbContextOptions<TContext>>()
                   .InstancePerLifetimeScope();

            builder.Register(context => context.Resolve<DbContextOptions<TContext>>())
                   .As<DbContextOptions>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<TContext>()
                   .AsSelf()
                   .InstancePerLifetimeScope();
        }


        private static void BuildContainer(ContainerBuilder builder, ILog log, IMainConfig config)
        {
            builder.RegisterInstance(log).As<ILog>().SingleInstance();

            builder.RegisterInstance(config).As<IMainConfig>().SingleInstance();
            builder.RegisterInstance(config.MyDatabase).As<IDatabaseConfig>().SingleInstance();
            builder.RegisterInstance(config.Log).As<ILogConfig>().SingleInstance();
            builder.RegisterType<BookRepository>().As<IBookRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BookLogic>().As<IBookLogic>().InstancePerLifetimeScope();

            var connectionString = new NpgsqlConnectionStringBuilder()
            {
                Host = config.MyDatabase.Host,
                Port = config.MyDatabase.Port,
                Username = config.MyDatabase.Username.ToUnsecureString(),
                Password = config.MyDatabase.Password.ToUnsecureString(),
                Database = config.MyDatabase.DatabaseName.ToUnsecureString(),
            }.ConnectionString;

            RegisterContext<BookContext>(builder, connectionString);
        }

        public static void Validate(ILifetimeScope container, ILog log)
        {
            ValidateResolvingSchema(container, log);

            var bookContext = container.Resolve<BookContext>();

            if (bookContext.Database.CanConnect())
            {
                log.Information($"Connected book database using {bookContext.Database.GetDbConnection().ConnectionString}");
            }
            else
            {
                throw new
                    InvalidOperationException($"Can't connect to the book database using '{bookContext.Database.GetDbConnection().ConnectionString}'");
            }
        }

        private static void ValidateResolvingSchema(ILifetimeScope container, ILog log)
        {
            var services = container
                           .ComponentRegistry
                           .Registrations
                           .SelectMany(x => x.Services)
                           .OfType<TypedService>()
                           .ToArray();

            foreach (var service in services.Where(x => !x.ServiceType.FullName.Contains("Actor")))
            {
                var logString = $"Resolving {service.Description} >";
                log.Debug(logString);
                var item = container.Resolve(service.ServiceType);
                log.Debug($"{new string(' ', logString.Length - service.Description.Length)} {item.GetType().FullName}");
            }

            log.Debug("DI schema is valid");
        }

        private static (IMainConfig config, ILog logger) Init()
        {
            IMainConfig config;

            try
            {
                config = ReadConfig();
            }
            catch (Exception e)
            {
                throw new InitializationException($"Can't read configuration entity: {e.Message}", e);
            }

            ILog log;

            try
            {
                log = new Log(config.Log.LogFilePath, config.Log.Level);
                log.Information($"Working in {Directory.GetCurrentDirectory()}");
                log.Information($"Log settings: '{log.LogPath} / {log.LogLevel}'");
            }
            catch (Exception e)
            {
                throw new InitializationException("Can't create logger", e);
            }

            return (config, log);
        }

        private static IMainConfig ReadConfig()
        {
            var currentDirectory = DirectoryPath.Current;

            var configText =
                currentDirectory.FindChildFile(ConfigFileName, ActionOnNotFound.ThrowNewException);

            var config = JsonConvert.DeserializeObject<MainConfigModel>(File.ReadAllText(configText))
                                    .ToMainConfig(currentDirectory);

            return config;
        }

        private static MainConfig ToMainConfig(this MainConfigModel model, DirectoryPath currentDirectory)
        {
            var myDatabase = new DatabaseConfig(model.MyDatabase.Host,
                                                (int)uint.Parse(model.MyDatabase.Port),
                                                model.MyDatabase.Username,
                                                model.MyDatabase.Password,
                                                model.MyDatabase.DatabaseName);

            var log = new LogConfig(currentDirectory.CombineFile(model.Log.LogFilePath),
                                    model.Log.Level);

            return new MainConfig(myDatabase, log);
        }

        private static IContainer Container { get; set; }
    }
}
