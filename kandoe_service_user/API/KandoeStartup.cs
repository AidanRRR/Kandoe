using System.Reflection;
using API.Extensions;
using API.Services;
using Autofac;
using AutoMapper;
using DAL;
using DAL.Configurations;
using DAL.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Models.Models;
using Swashbuckle.Swagger.Model;

namespace API
{
    public class KandoeStartup
    {
        public KandoeStartup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("jwtConfig.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Mediater Links
            services.AddTransient<IUserRepository, UserRepository>();

            // Mediator
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<SingleInstanceFactory>(sp => t => sp.GetService(t));
            services.AddTransient<MultiInstanceFactory>(sp => t => sp.GetServices(t));
            services.AddMediatorHandlers(typeof(KandoeStartup).GetTypeInfo().Assembly);

            // Automapper
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            // Auth Service
            services.AddTransient<IAuthService, AuthService>();
            services.Configure<JwtConfig>(Configuration);

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

            // Mongodb
            services.Configure<MongoSettings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            // Swagger
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {

                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "User Service Backend API",
                    Description = "(c) Team Pijl 2017",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
                options.CustomSchemaIds(x => x.FullName);
            });

            // Mvc
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSwagger();
            app.UseSwaggerUi();

            app.UseCors("CorsPolicy");

            app.UseMvc();
        }
    }
}