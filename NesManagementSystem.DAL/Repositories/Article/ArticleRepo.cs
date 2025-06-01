﻿using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.DBContext;

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
    
    public async Task<List<NewsArticle>> GetArticlesByCategoryIdAsync(short categoryId)
    {
        return await _context.NewsArticles
            .Where(a => a.CategoryID == categoryId)
            .Include(a => a.Tags)
            .ToListAsync();
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

    public async Task UpdateArticleAsync(NewsArticle article, List<int> tagIds)
    {
        var result = await _context.NewsArticles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(x => x.NewsArticleID == article.NewsArticleID);

        if (result == null) return;

        result.NewsTitle = article.NewsTitle;
        result.Headline = article.Headline;
        result.NewsContent = article.NewsContent;
        result.NewsSource = article.NewsSource;
        result.ModifiedDate = article.ModifiedDate;
        result.CategoryID = article.CategoryID;
        
        result.Tags?.Clear();
        if (tagIds != null && tagIds.Count > 0)
        {
            var tags = await _context.Tags.Where(t => tagIds.Contains(t.TagID)).ToListAsync();
            foreach (var tag in tags)
            {
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

    public async Task<List<NewsArticle>> GetArticlesyncOderByDescending()
    {
        var result = await _context.NewsArticles
            .Include(a => a.Tags).Include(a=>a.Category)
            .OrderByDescending(a => a.CreatedDate)
            .ToListAsync();
        return result;
    }

    public async Task<List<NewsArticle>> GetArticleByDateRange(DateTime? startDate, DateTime? endDate)
    {
       var result = await _context.NewsArticles
            .Include(a => a.Tags)
            .Include(a => a.Category)
            .Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate)
            .OrderByDescending(a=>a.CreatedDate).ToListAsync();
       
        return result;
    }
}