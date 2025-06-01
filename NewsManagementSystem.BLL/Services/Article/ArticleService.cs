using BusinessObject.Entities;
using NewsManagementSystem.DAL.Repositories.Article;
using NewsManagementSystem.DAL.Repositories.Tag;

namespace NewsManagementSystem.BLL.Services.Article;

public class ArticleService : IArticleService
{
    private readonly IArticleRepo _articleRepo;
    private readonly ITagRepo _tagRepo;

    public ArticleService(IArticleRepo articleRepo, ITagRepo tagRepo)
    {
        _articleRepo = articleRepo;
        _tagRepo = tagRepo;
    }

    public Task<List<NewsArticle>> GetArticlesync() => _articleRepo.GetArticlesync();
    public async Task<List<NewsArticle>> GetActiveArticlesAsync()
    {
        return await _articleRepo.GetActiveArticlesAsync();
    }


    public Task<NewsArticle?> GetArticleByNameAsync(string name) => _articleRepo.GetArticleByNameAsync(name);

    public async Task<NewsArticle?> GetArticleByIdWithTagsAsync(string id)
    {
        var list = await _articleRepo.GetArticlesync();
        return list.FirstOrDefault(x => x.NewsArticleID == id);
    }
    
    public async Task<List<NewsArticle>> GetArticlesByCategoryIdAsync(short categoryId)
    {
        return await _articleRepo.GetArticlesByCategoryIdAsync(categoryId);
    }

    public Task CreateArticleAsync(NewsArticle article) => _articleRepo.CreateArticleAsync(article);

    public async Task CreateArticleWithTagsAsync(NewsArticle article, List<int> tagIds)
    {
        var tags = await _tagRepo.GetTagsByIdsAsync(tagIds);
        article.Tags = tags;
        await _articleRepo.CreateArticleAsync(article);
    }
    
    public async Task UpdateArticleWithTagsAsync(NewsArticle article, List<int> tagIds)
    {
        await _articleRepo.UpdateArticleAsync(article, tagIds);
    }


    public Task DeleteArticleAsync(NewsArticle article) => _articleRepo.DeleteArticleAsync(article);

    public async Task DeleteArticleByIdAsync(string id)
    {
        var article = await GetArticleByIdWithTagsAsync(id);
        if (article != null)
        {
            await _articleRepo.DeleteArticleAsync(article);
        }
    }
}
