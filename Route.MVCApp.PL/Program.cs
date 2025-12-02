using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Route.MVCApp.BLL.Common.Service.Attachments;
using Route.MVCApp.BLL.Services.Departments;
using Route.MVCApp.BLL.Services.Employees;
using Route.MVCApp.DAL.Models.Identity;
using Route.MVCApp.DAL.Persistence.Data.Contexts;
using Route.MVCApp.DAL.Persistence.Repositories.Departments;
using Route.MVCApp.DAL.Persistence.Repositories.Employees;
using Route.MVCApp.DAL.Persistence.UnitOfWork;
using Route.MVCApp.PL.Mapping;

namespace Route.MVCApp.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services [Add Service To DI Container (Services)]

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>((optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            builder.Services.AddTransient<IAttachmentService, AttachmentService>();

            builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>((option) =>
            {
                //option.Password.RequiredLength = 6;
                //option.Password.RequireNonAlphanumeric = true;
                //option.Password.RequireUppercase = true;
                //option.Password.RequireLowercase = true;
                //option.Password.RequireDigit = true;
                //option.Password.RequiredUniqueChars = 1;

                //option.User.RequireUniqueEmail = true;
                //option.User.AllowedUserNameCharacters = "ABZET";
                //option.Lockout.MaxFailedAccessAttempts = 3;
                //option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4);

            })
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();

            #endregion

            var app = builder.Build();

            #region Configure Kestrel MiddleWares [Pipelines]
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection(); // Redirect HTTP Protocol to HTTPS Protocol
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Register}/{id?}");
            #endregion

            app.Run();
        }
    }
}
