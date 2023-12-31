using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Toast;
using ChadWebCalendar.Data;
using ChadWebCalendar.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
using System;
using System.IO;

namespace ChadWebCalendar
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChadCalendar");
            Directory.CreateDirectory(path);
            services.AddDbContext<ApplicationContext>(options => options.UseSqlite($"Data Source = { Path.Combine(path, "Calendar.db")}"));

            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureCreated();
            }

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<Radzen.NotificationService>();
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();

            services.AddSingleton<Data.Services.TaskService>();
            services.AddSingleton<Data.Services.EventService>();
            services.AddSingleton<Data.Services.ProjectService>();

            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();
            services.AddScoped<TooltipService>();
            services.AddBlazoredModal();
            services.AddBlazoredToast();
            services.AddAntDesign();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
