using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BLL.Services.Category;
using NewsManagementSystem.BLL.Services.Tag;
using NewsManagementSystem.DAL.DBContext;
using NewsManagementSystem.DAL.Repositories.Article;
using NewsManagementSystem.DAL.Repositories.Category;
using NewsManagementSystem.DAL.Repositories.Tag;

namespace NewsManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            //Register DB
            builder.Services.AddDbContext<FUNewsManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionStringDB")));

            //Register repositories
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<IArticleRepo, ArticleRepo>();
            builder.Services.AddScoped<ITagRepo, TagRepo>();
            
            //Register services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ITagService, TagService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Category}/{action=ListCategories}/{id?}");

            app.Run();
        }
    }
}
