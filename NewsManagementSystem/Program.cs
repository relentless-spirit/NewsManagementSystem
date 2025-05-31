using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.BLL.Services.Category;
using NewsManagementSystem.BLL.Services.Account;
using NewsManagementSystem.DAL.DBContext;
using NewsManagementSystem.DAL.Repositories.Category;
using NewsManagementSystem.DAL.Repositories.Account;

namespace NewsManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register DbContext
            builder.Services.AddDbContext<FUNewsManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionStringDB")));

            // Enable Session
            builder.Services.AddSession(); // 🟢 Thêm dòng này để dùng session

            // Register Category Repository and Service
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            // 🔐 Register Account Repository and Service for Login
            builder.Services.AddScoped<IAccountRepo, AccountRepo>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Category}/{action=ListCategories}/{id?}");

            app.Run();
        }
    }
}
