using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobPortal.Areas.Identity.Data;
namespace JobPortal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
                        var connectionString = builder.Configuration.GetConnectionString("JobPortalContextConnection") ?? throw new InvalidOperationException("Connection string 'JobPortalContextConnection' not found.");

                                    builder.Services.AddDbContext<JobPortalContext>(options =>
                options.UseSqlServer(connectionString));

                                                builder.Services.AddDefaultIdentity<JobPortalUser>()
                .AddEntityFrameworkStores<JobPortalContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.MapRazorPages();
            app.UseRouting();
                        app.UseAuthentication();;

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}