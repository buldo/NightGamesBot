namespace Ngb.Web
{
    using Ngb.Bot;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using Ngb.Web.Data;
    using Ngb.Web.Helpers;
    using Ngb.Web.Models;
    using Ngb.Web.Services;


    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                try
                {
                    builder.AddUserSecrets();
                }
                catch (System.Exception)
                {
                    // Ничего страшного, если не нашли. Может быть отлаживаемся на сервера
                }
            }

            
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString;
            string herokuDatabaseUrl = Configuration["DATABASE_URL"];
            if (string.IsNullOrWhiteSpace(herokuDatabaseUrl))
            {
                var receiveDataString = Configuration["RECEIVE_DATA"];
                if (string.IsNullOrWhiteSpace(receiveDataString))
                {
                    // Мы в локальной среде
                    connectionString = Configuration.GetConnectionString("DefaultConnection");
                }
                else
                {
                    // Мы в процессе сборки на хероку
                    dynamic receiveData = JsonConvert.DeserializeObject(receiveDataString);
                    connectionString = receiveData.push_metadata.env.DefaultConnection + "SSL Mode=Require;Trust Server Certificate=true;Pooling=false;";
                }
            }
            else
            {
                // По простому получили все данные и работаем
                connectionString = DbHelpers.DatabaseUrlToPostgreConnectionString(herokuDatabaseUrl);
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            var bot = new GamesBot(Configuration["Token"]);
            if (bool.Parse(Configuration["IsPollingEnabled"]?? bool.FalseString))
            {
                bot.StartLongPooling();
            }
            else
            {
                bot.EnableWebHook(Configuration["HookUrl"]);
            }
            services.AddSingleton<IUpdateMessagesProcessor>(bot);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
