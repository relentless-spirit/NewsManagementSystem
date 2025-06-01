using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BLL.Services.Category;
using NewsManagementSystem.BLL.Services.SystemAccount;
using NewsManagementSystem.BLL.Services.Tag;
using NewsManagementSystem.DAL.DBContext;
using NewsManagementSystem.DAL.Repositories.Article;
using NewsManagementSystem.DAL.Repositories.Category;
using NewsManagementSystem.DAL.Repositories.Tag;
using NewsManagementSystem.DAL.SystemAccount;
using NewsManagementSystem.Mapper;
using NewsManagementSystem.Models;
using NewsManagementSystem.Validation;

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
            builder.Services.AddScoped<ISystemAccountRepo, SystemAccountRepo>();

            //Register services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();

            //Register validators
            builder.Services.AddScoped<IValidator<CreateAccountRequest>, CreateAccountReqValidator>();
            builder.Services.AddScoped<IValidator<UpdateSystemAccountRequest>, UpdateSystemAccountRequestValidator>();

            //Register AutoMapper
            builder.Services.AddAutoMapper(typeof(AccountProfile));

            // ĐẶT AddSession TRƯỚC khi Build()
            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

           
            app.UseSession();

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
