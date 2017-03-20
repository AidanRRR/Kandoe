using System;
using System.Reflection;
using API.Extensions;
using Autofac;
using AutoMapper;
using DAL.Configurations;
using DAL.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models.Models.Events;
using MongoDB.Bson.Serialization;
using Swashbuckle.Swagger.Model;
using Newtonsoft.Json;
using BL;
using BL.Mail;
using Models.Models.Session;

namespace API
{
    public class KandoeStartup
    {
        public KandoeStartup(IHostingEnvironment env)
        {
            var configFile = env.IsProduction() ? "appsettings.prod.json" : "appsettings.dev.json";
            Console.WriteLine("Gebruikte config file: " + configFile);
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Contract resolver voor de JSON zorgt voor 
            // camelcase serialisatie van entiteiten bij SignalR
            // Zie: http://stackoverflow.com/questions/37832165/signalr-net-core-camelcase-json-contract-resolver
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            services.Add(new ServiceDescriptor(typeof(JsonSerializer), provider => serializer, ServiceLifetime.Transient));

            // Mail Service Configuration
            services.Configure<MailSettings>(options =>
            { 
                options.From = Configuration.GetSection("MailSettings:From").Value;
                options.Host = Configuration.GetSection("MailSettings:Host").Value;
                options.Port = int.Parse(Configuration.GetSection("MailSettings:Port").Value);
                options.UseSsl = bool.Parse(Configuration.GetSection("MailSettings:UseSsl").Value);
                options.User = Configuration.GetSection("MailSettings:User").Value;
                options.Password = Configuration.GetSection("MailSettings:Password").Value;   

                // Na migreren naar nieuwe .NET Core werkt bovenstaande configuratie niet meer. Config bug.
                //options.From = "kandoe@rypens.be";
                //options.Host = "send.one.com";
                //options.Port = 465;
                //options.UseSsl = true;
                //options.User = "kandoe@rypens.be";
                //options.Password = "kandoeKANDOE1";


            });

            // MongoDB Service Configuration
            services.Configure<MongoSettings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            // ServiceMgr
            services.Configure<ServiceManager.ServiceManagerConfig>(options => {
                options.ThemeServiceUrl = Configuration.GetSection("Services:Theme").Value;
                options.AuthServiceUrl = Configuration.GetSection("Services:Auth").Value;
                options.UserServiceUrl = Configuration.GetSection("Services:User").Value;
            });

            // DI Container
            services.AddTransient<ISessionEventsRepository, SessionEventsRepository>();
            services.AddTransient<IMailSender, MailSender>();

            // MongoDB ClassPaths
            // Registreren van classes voor deserialisatie
            BsonClassMap.RegisterClassMap<ChatMessageEvent>();
            BsonClassMap.RegisterClassMap<CardPickEvent>();
            BsonClassMap.RegisterClassMap<MoveEvent>();
            BsonClassMap.RegisterClassMap<GameStartEvent>();
            BsonClassMap.RegisterClassMap<SessionStartEvent>();
            BsonClassMap.RegisterClassMap<TurnStartEvent>();
            BsonClassMap.RegisterClassMap<SessionEndEvent>();

            BsonClassMap.RegisterClassMap<Session>();
            BsonClassMap.RegisterClassMap<Snapshot>();

            // Automapper
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            // Mediator
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<SingleInstanceFactory>(sp => t => sp.GetService(t));
            services.AddTransient<MultiInstanceFactory>(sp => t => sp.GetServices(t));
            services.AddMediatorHandlers(typeof(KandoeStartup).GetTypeInfo().Assembly);

            // Container
            var builder = new ContainerBuilder();
            var container = builder.Build();

            // Cross origin
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                  b => b.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            // Swagger
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {

                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Session Service Backend API",
                    Description = "(c) Team Pijl 2017",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
                options.CustomSchemaIds(x => x.FullName);
            });

            // SignalR
            services.AddAuthorization(opt => {
            });
            services.AddSignalR(options =>
            {
                options.EnableJSONP = true;
                options.Hubs.EnableDetailedErrors = true;
            });

            // Mvc
            services.AddMvc();

            // Eigen services registreren
            services.AddSingleton(typeof(ISessionManager), typeof(SessionManager));
            services.AddSingleton(typeof(ITurnTicker), typeof(TurnTicker));
            services.AddSingleton(typeof(ISessionTicker), typeof(SessionTicker));
            services.AddSingleton(typeof(IServiceManager), typeof(ServiceManager));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSwagger();
            app.UseSwaggerUi();

            app.UseStaticFiles();
            app.UseWebSockets();
            app.UseSignalR();

            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}