using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.DBContext;
using NewsManagementSystem.DAL.Entities;

namespace NewsManagementSystem.DAL.Repositories.Article;

public class ArticleRepo : IArticleRepo
{
    private readonly FUNewsManagementContext _context;

    public ArticleRepo(FUNewsManagementContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<NewsArticle>> GetArticlesync()
    {
        return await _context.NewsArticles
            .Include(a => a.Tags)
            .ToListAsync();
    }

    public async Task<NewsArticle?> GetArticleByNameAsync(string name)
    {
        return await _context.NewsArticles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.NewsTitle == name);
    }

    public async Task CreateArticleAsync(NewsArticle article)
    {
        if (article.Tags != null && article.Tags.Any())
        {
            foreach (var tag in article.Tags)
            {
                _context.Entry(tag).State = EntityState.Unchanged;
            }
        }

        _context.NewsArticles.Add(article);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateArticleAsync(NewsArticle article)
    {
        var result = await _context.NewsArticles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.NewsArticleID == article.NewsArticleID);

        if (result == null) return;

        _context.Entry(result).CurrentValues.SetValues(article);
        
        result.Tags.Clear();
        if (article.Tags != null && article.Tags.Any())
        {
            foreach (var tag in article.Tags)
            {
                _context.Entry(tag).State = EntityState.Unchanged;
                result.Tags.Add(tag);
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteArticleAsync(NewsArticle article)
    {
        var result = await _context.NewsArticles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.NewsArticleID == article.NewsArticleID);

        if (result == null) return;

        result.Tags.Clear();
        _context.NewsArticles.Remove(result);
        await _context.SaveChangesAsync();
    }
}