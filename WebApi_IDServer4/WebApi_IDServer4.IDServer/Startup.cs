using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi_IDServer4.IDServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            var connectionString = config.GetSection("connectionString").Value;
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddMvc();
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                //.AddInMemoryApiResources(Config.GetAllApiResources())
                //.AddInMemoryClients(Config.GetAllClients())
                //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddTestUsers(Config.GetUsers())
                ///Configuation store for clients and resouces
                .AddConfigurationStore(options=> {
                    options.ConfigureDbContext = b =>
                    b.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationAssembly));
                })
                /// operational store for token, consents, code etc
                .AddOperationalStore(options=> {
                    options.ConfigureDbContext = b =>
                  b.UseSqlServer(connectionString,
                  sql => sql.MigrationsAssembly(migrationAssembly));
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //IntializeIdentityServerDatabase(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

        }

        private void IntializeIdentityServerDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetService<IServiceScopeFactory>().CreateScope())
            {
                //serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                //context.Database.Migrate();

                ///Seed the data
                if (context.Clients.Any())
                {
                    foreach(var client in Config.GetAllClients().Where(c=>c.ClientId== "swaggerapiui"))
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var item in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(item.ToEntity());
                    }
                    context.SaveChanges();

                }

                if (!context.ApiResources.Any())
                {
                    foreach (var item in Config.GetAllApiResources())
                    {
                        context.ApiResources.Add(item.ToEntity());
                    }
                    context.SaveChanges();

                }
            }
        }
    }
}
