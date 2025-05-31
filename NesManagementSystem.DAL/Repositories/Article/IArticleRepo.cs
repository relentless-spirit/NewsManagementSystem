using BusinessObject.Entities;

namespace NewsManagementSystem.DAL.Repositories.Article;

public interface IArticleRepo
{
    Task<List<NewsArticle>> GetArticlesync();
    Task<NewsArticle?> GetArticleByNameAsync(string name);
    Task CreateArticleAsync(NewsArticle category);
    Task UpdateArticleAsync(NewsArticle category);
    Task DeleteArticleAsync(NewsArticle category);
}