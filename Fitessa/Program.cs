using Fitessa.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;
using Fitessa.Services.Services;
using Fitessa.Middleware;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Fitessa.Hubs;

namespace Fitessa.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Fitessa.Data")));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            
            builder.Services.AddScoped<ISubscriptionPlanService, Fitessa.Services.Services.SubscriptionPlanService>();
            builder.Services.AddScoped<IWorkoutProgramService, Fitessa.Services.Services.WorkoutProgramService>();
            builder.Services.AddScoped<IExerciseService, Fitessa.Services.Services.ExerciseService>();
            builder.Services.AddScoped<IMeasurementLogService, Fitessa.Services.Services.MeasurementLogService>();
            builder.Services.AddScoped<IMealPlanService, Fitessa.Services.Services.MealPlanService>();
            builder.Services.AddScoped<IProgressInsightsService, Fitessa.Services.Services.ProgressInsightsService>();
            builder.Services.AddScoped<IEmailService, Fitessa.Services.Services.EmailService>();
            builder.Services.AddScoped<INutritionApiService, Fitessa.Services.Services.NutritionApiService>();
            builder.Services.AddScoped<IFitnessAnalyticsService, Fitessa.Services.Services.FitnessAnalyticsService>();
            builder.Services.AddScoped<INotificationService, Fitessa.Services.Services.NotificationService>();
            
            builder.Services.AddAutoMapper(typeof(Program));
            
            builder.Services.AddHttpClient();
            
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IConverter, SynchronizedConverter>(_ => new SynchronizedConverter(new PdfTools()));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapHub<Fitessa.Hubs.NotificationHub>("/notificationHub");

            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    DbInitializer.Seed(services);
                }
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }

            app.Run();
        }
    }
}
