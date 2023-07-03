using Microsoft.AspNetCore.Authentication.Cookies;

namespace BusinessManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure services for HttpContextAccessor
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Configure distributed memory cache and session options
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(15);
            });

            // Configure authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Google";
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Home/Index"; // Set the login path
            })
            .AddGoogle(options =>
            {
                options.ClientId = "451393101026-kdlt6ln1372g7atlfr5unuh1v1ca85gq.apps.googleusercontent.com";
                options.ClientSecret = "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";
                options.SaveTokens = true; // Enable saving tokens for later use
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication(); // Add authentication middleware
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}





