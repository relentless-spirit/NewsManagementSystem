namespace NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.DAL.Entities;

public interface IArticleService
{
    Task<List<NewsArticle>> GetArticlesync();
    Task<NewsArticle?> GetArticleByNameAsync(string name);
    Task<NewsArticle?> GetArticleByIdWithTagsAsync(string id);
    Task CreateArticleAsync(NewsArticle article);
    Task CreateArticleWithTagsAsync(NewsArticle article, List<int> tagIds);
    Task UpdateArticleAsync(NewsArticle article);
    Task UpdateArticleWithTagsAsync(NewsArticle article, List<int> tagIds);
    Task DeleteArticleAsync(NewsArticle article);
    Task DeleteArticleByIdAsync(string id);
}